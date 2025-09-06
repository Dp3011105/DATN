using BE.models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FE.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        private readonly IHoaDonService _hoaDonService;
        private readonly IProductService _productService;

        // Các status đúng theo DB (giữ nguyên “key” DB để lọc/so sánh)
        private static readonly string[] DbStatuses = new[]
        {
            "Chua_Xac_Nhan",
            "Da_Xac_Nhan",
            "Dang_Xu_Ly",
            "Dang_Giao_Hang",
            "Hoan_Thanh",
            "Huy_Don"
        };

        public QuanLyDonHangController(IHoaDonService hoaDonService, IProductService productService)
        {
            _hoaDonService = hoaDonService;
            _productService = productService;
        }

        // ====== DANH SÁCH ======
        [HttpGet]
        public async Task<IActionResult> Index(string tuKhoa = "", string trangThai = "TẤT CẢ")
        {
            var vm = new QuanLyDonHangViewModel
            {
                TuKhoa = tuKhoa?.Trim() ?? "",
                TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "TẤT CẢ" : trangThai.Trim(),
                // Hiển thị “TẤT CẢ” + danh sách key DB để lọc đúng với DB
                TrangThaiList = new List<string> { "TẤT CẢ" }
                    .Concat(DbStatuses)
                    .ToList()
            };

            try
            {
                var list = (await _hoaDonService.GetAllAsync())?.ToList() ?? new();

                // Lọc từ khoá theo Mã HĐ / Tên KH
                if (!string.IsNullOrWhiteSpace(vm.TuKhoa))
                {
                    var kw = vm.TuKhoa;
                    list = list.Where(hd =>
                           (hd.Ma_Hoa_Don?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                        || (hd.KhachHang?.Ho_Ten?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                    ).ToList();
                }

                // Lọc trạng thái: so sánh trực tiếp với giá trị trong DB
                if (!vm.TrangThai.Equals("TẤT CẢ", StringComparison.OrdinalIgnoreCase))
                {
                    list = list
                        .Where(hd => string.Equals(hd.Trang_Thai, vm.TrangThai, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Ưu tiên Chua_Xac_Nhan, sau đó mới đến thời gian tạo
                vm.DanhSachHoaDon = list
                    .OrderBy(h => string.Equals(h.Trang_Thai, "Chua_Xac_Nhan", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                    .ThenByDescending(h => h.Ngay_Tao)
                    .ToList();
            }
            catch
            {
                vm.DanhSachHoaDon = new();
            }

            return View(vm);
        }

        // ====== CHI TIẾT ======
        [HttpGet]
        public async Task<IActionResult> ChiTiet(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null) return NotFound();

            var vm = new ChiTietHoaDonViewModel
            {
                HoaDon = hd,
                ChiTiets = hd.HoaDonChiTiets?
                    .OrderBy(x => x.ID_HoaDon_ChiTiet)
                    .ToList() ?? new()
            };

            return View(vm);
        }

        // ====== CẬP NHẬT TRẠNG THÁI ======
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan(int id)
        {
            var ok = await _hoaDonService.UpdateTrangThaiAsync(id, "Dang_Giao_Hang", null);
            TempData["msg"] = ok ? "Đã chuyển đơn sang trạng thái: Đang giao hàng." : "Cập nhật thất bại.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiaoHangThanhCong(int id)
        {
            var ok = await _hoaDonService.UpdateTrangThaiAsync(id, "Hoan_Thanh", null);
            TempData["msg"] = ok ? "Đã xác nhận giao hàng thành công." : "Cập nhật thất bại.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Huy(int id, string lyDo)   // <-- thêm lyDo
        {
            if (string.IsNullOrWhiteSpace(lyDo))
            {
                TempData["msg"] = "Vui lòng nhập lý do hủy.";
                return RedirectToAction(nameof(Index));
            }

            var ok = await _hoaDonService.UpdateTrangThaiAsync(id, "Huy_Don", lyDo);
            TempData["msg"] = ok ? "Đã hủy đơn." : "Hủy đơn thất bại.";
            return RedirectToAction(nameof(Index));
        }
    }

    // ===== ViewModels =====
    public class QuanLyDonHangViewModel
    {
        public List<HoaDon> DanhSachHoaDon { get; set; } = new();
        public string TuKhoa { get; set; } = "";
        public string TrangThai { get; set; } = "TẤT CẢ";
        public List<string> TrangThaiList { get; set; } = new();
    }

    public class ChiTietHoaDonViewModel
    {
        public HoaDon HoaDon { get; set; } = default!;
        public List<HoaDonChiTiet> ChiTiets { get; set; } = new();
    }
}
