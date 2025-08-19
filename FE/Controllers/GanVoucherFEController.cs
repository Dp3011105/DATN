using FE.Models;
using FE.Service;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class GanVoucherFEController : Controller
    {
        private readonly IGanVoucherService _service;

        public GanVoucherFEController(IGanVoucherService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new GanVoucherViewModel();

            try
            {
                // Load dữ liệu
                vm.KhachHangs = await _service.GetAllKhachHangAsync();
                vm.Vouchers = await _service.GetAllVouchersAsync();
                vm.TopVipKhachHangs = await _service.GetTop10VipAsync();

                if (!vm.KhachHangs.Any())
                {
                    TempData["Warning"] = "Không tìm thấy khách hàng nào trong hệ thống.";
                }

                if (!vm.Vouchers.Any())
                {
                    TempData["Warning"] = "Không tìm thấy voucher nào còn số lượng.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải dữ liệu: {ex.Message}";

                // Set empty lists để tránh null reference
                vm.KhachHangs ??= new List<BE.models.KhachHang>();
                vm.Vouchers ??= new List<BE.models.Voucher>();
                vm.TopVipKhachHangs ??= new List<BE.models.KhachHang>();
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GanVoucher(GanVoucherViewModel vm)
        {
            try
            {
                // Validation phía server
                if (vm.ID_Khach_Hang == null || vm.ID_Khach_Hang.Count == 0)
                {
                    TempData["Error"] = "Vui lòng chọn ít nhất một khách hàng.";
                    return RedirectToAction("Index");
                }

                if (vm.ID_Voucher == null || vm.ID_Voucher.Count == 0)
                {
                    TempData["Error"] = "Vui lòng chọn ít nhất một voucher.";
                    return RedirectToAction("Index");
                }

                if (vm.SoLuong <= 0)
                {
                    TempData["Error"] = "Số lượng phải lớn hơn 0.";
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrWhiteSpace(vm.GhiChu))
                {
                    TempData["Error"] = "Vui lòng nhập ghi chú.";
                    return RedirectToAction("Index");
                }

                // Gọi service và nhận kết quả
                var result = await _service.GanVoucherAsync(vm);

                if (result.Success)
                {
                    if (result.HasWarnings)
                    {
                        TempData["Warning"] = result.Message; // Hiển thị màu vàng cho warning
                    }
                    else
                    {
                        TempData["Result"] = result.Message; // Hiển thị màu xanh cho success
                    }
                }
                else
                {
                    TempData["Error"] = result.Message; // Hiển thị màu đỏ cho error
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"❌ Lỗi không xác định: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}