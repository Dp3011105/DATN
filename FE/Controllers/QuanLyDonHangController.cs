using BE.models;
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

        public QuanLyDonHangController(IHoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }

        // ====== DANH SÁCH (server render) ======
        [HttpGet]
        public async Task<IActionResult> Index(string tuKhoa = "", string trangThai = "TẤT CẢ")
        {
            var vm = new QuanLyDonHangViewModel
            {
                TuKhoa = tuKhoa ?? "",
                TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "TẤT CẢ" : trangThai,
                TrangThaiList = new List<string>
                {
                    "TẤT CẢ","Chờ xác nhận","Chờ giao","Vận chuyển","Hoàn thành","Đã huỷ"
                }
            };

            try
            {
                var list = (await _hoaDonService.GetAllAsync())?.ToList() ?? new();

                // Lọc từ khoá
                if (!string.IsNullOrWhiteSpace(vm.TuKhoa))
                {
                    var kw = vm.TuKhoa.Trim();
                    list = list.Where(hd =>
                           (hd.Ma_Hoa_Don?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                        || (hd.KhachHang?.Ho_Ten?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                    ).ToList();
                }

                // Lọc trạng thái
                if (!IsTatCa(vm.TrangThai))
                {
                    var db = MapTrangThaiToDb(vm.TrangThai);
                    if (!string.IsNullOrEmpty(db))
                        list = list.Where(hd => string.Equals(hd.Trang_Thai, db, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                vm.DanhSachHoaDon = list
                    .OrderByDescending(h => h.Ngay_Tao)
                    .ToList();
            }
            catch
            {
                vm.DanhSachHoaDon = new();
            }

            return View(vm);
        }

        // ====== CHI TIẾT (server render) ======
        [HttpGet]
        public async Task<IActionResult> ChiTiet(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);   // dùng chữ ký hiện có của service
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan(int id)
        {
            try
            {
                var hd = await _hoaDonService.GetByIdAsync(id);
                if (hd == null)
                {
                    TempData["msg"] = "Không tìm thấy hóa đơn.";
                    return RedirectToAction(nameof(Index));
                }

                // chỉ xác nhận nếu đang chờ
                if (string.Equals(hd.Trang_Thai, "Chua_Xac_Nhan", StringComparison.OrdinalIgnoreCase))
                {
                    hd.Trang_Thai = "Da_Xac_Nhan";
                    await _hoaDonService.UpdateAsync(hd.ID_Hoa_Don, hd);
                    TempData["msg"] = $"Đã xác nhận đơn {hd.Ma_Hoa_Don}.";
                }
                else
                {
                    TempData["msg"] = "Đơn không ở trạng thái Chờ xác nhận.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = $"Lỗi xác nhận: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // ===== HỦY ĐƠN: đặt Trang_Thai = Huy_Don =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Huy(int id)
        {
            try
            {
                var hd = await _hoaDonService.GetByIdAsync(id);
                if (hd == null)
                {
                    TempData["msg"] = "Không tìm thấy hóa đơn.";
                    return RedirectToAction(nameof(Index));
                }

                if (!string.Equals(hd.Trang_Thai, "Hoan_Thanh", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(hd.Trang_Thai, "Huy_Don", StringComparison.OrdinalIgnoreCase))
                {
                    hd.Trang_Thai = "Huy_Don";
                    await _hoaDonService.UpdateAsync(hd.ID_Hoa_Don, hd);
                    TempData["msg"] = $"Đã hủy đơn {hd.Ma_Hoa_Don}.";
                }
                else
                {
                    TempData["msg"] = "Đơn đã hoàn thành hoặc đã hủy.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = $"Lỗi hủy đơn: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
        // ---- Helpers ----
        private static bool IsTatCa(string s) =>
            string.IsNullOrWhiteSpace(s) ||
            s.Trim().Equals("TẤT CẢ", StringComparison.OrdinalIgnoreCase) ||
            s.Trim().Equals("Tất cả", StringComparison.OrdinalIgnoreCase);

        private static string MapTrangThaiToDb(string hienThi) => hienThi switch
        {
            "Chờ xác nhận" or "Đang xử lý" => "Chua_Xac_Nhan",
            "Chờ giao" => "Da_Xac_Nhan",
            "Vận chuyển" => "Dang_Giao_Hang",
            "Hoàn thành" or "Đã giao" or "Đã thanh toán" or "Hoàn tất"
                                             => "Hoan_Thanh",
            "Đã huỷ" => "Huy_Don",
            _ => string.IsNullOrWhiteSpace(hienThi) ? "" : hienThi
        };
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
