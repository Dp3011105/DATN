using Microsoft.AspNetCore.Mvc;
using Service.IService;
using BE.models;
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

        // Trang danh sách + filter
        public async Task<IActionResult> Index(string tuKhoa = "", string trangThai = "TẤT CẢ")
        {
            var viewModel = new QuanLyDonHangViewModel();
            try
            {
                var hoaDonList = (await _hoaDonService.GetAllAsync())?.ToList() ?? new();

                if (!string.IsNullOrWhiteSpace(tuKhoa))
                {
                    hoaDonList = hoaDonList.Where(hd =>
                        (hd.KhachHang?.Ho_Ten?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) == true) ||
                        (hd.Ma_Hoa_Don?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) == true)).ToList();
                }

                if (!string.IsNullOrEmpty(trangThai) && !IsTatCa(trangThai))
                {
                    var mapped = MapTrangThaiToDatabase(trangThai);
                    if (!string.IsNullOrEmpty(mapped) && mapped != trangThai)
                        hoaDonList = hoaDonList.Where(hd => hd.Trang_Thai == mapped).ToList();
                }

                viewModel.DanhSachHoaDon = hoaDonList;
            }
            catch { /* giữ UI chạy kể cả khi lỗi */ }

            viewModel.TuKhoa = tuKhoa;
            viewModel.TrangThai = string.IsNullOrEmpty(trangThai) ? "TẤT CẢ" : trangThai;
            viewModel.TrangThaiList = new List<string>
            {
                "TẤT CẢ",
                "Chờ xác nhận",
                "Chờ giao",
                "Vận chuyển",
                "Hoàn thành",
                "Đã huỷ"
            };
            return View(viewModel);
        }

        // Trang chi tiết 1 đơn
        public async Task<IActionResult> ChiTiet(int id)
        {
            var hoaDon = await _hoaDonService.GetByIdAsync(id);
            if (hoaDon == null) return NotFound();

            var vm = new ChiTietHoaDonViewModel
            {
                HoaDon = hoaDon,
                ChiTiets = hoaDon.HoaDonChiTiets?.ToList() ?? new()
            };

            return View(vm);
        }

        // API trả JSON để render bảng (dùng AJAX)
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetDanhSach(string tuKhoa = "", string trangThai = "TẤT CẢ")
        {
            try
            {
                var hoaDons = await _hoaDonService.GetAllAsync();
                var list = new List<object>();

                if (hoaDons != null)
                {
                    var mapped = MapTrangThaiToDatabase(trangThai);
                    list = hoaDons
                        .Where(hd =>
                            (string.IsNullOrEmpty(tuKhoa) ||
                                hd.KhachHang?.Ho_Ten?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) == true ||
                                hd.Ma_Hoa_Don?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) == true) &&
                            (IsTatCa(trangThai) || (!string.IsNullOrEmpty(mapped) && hd.Trang_Thai == mapped))
                        )
                        .Select(hd => new
                        {
                            ID_Hoa_Don = hd.ID_Hoa_Don,
                            Ma_Hoa_Don = hd.Ma_Hoa_Don,
                            Tong_Tien = hd.Tong_Tien,
                            KhachHang = hd.KhachHang?.Ho_Ten ?? "Khách lẻ",
                            Ngay_Tao = hd.Ngay_Tao.ToString("dd/MM/yyyy HH:mm"),
                            HinhThucThanhToan = hd.HinhThucThanhToan?.Phuong_Thuc_Thanh_Toan ?? "N/A",
                            Trang_Thai = hd.Trang_Thai,
                            TrangThaiHienThi = MapTrangThaiFromDatabase(hd.Trang_Thai)
                        })
                        .Cast<object>()
                        .ToList();
                }

                return Json(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Xác nhận / chuyển trạng thái (gọi service nhẹ)
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> CapNhatTrangThaiLight([FromBody] CapNhatTrangThaiRequest req)
        {
            try
            {
                if (req == null) return Json(new { success = false, message = "Body rỗng" });
                var hoaDon = await _hoaDonService.GetByIdAsync(req.HoaDonId);
                if (hoaDon == null) return Json(new { success = false, message = "Không tìm thấy hóa đơn" });

                var newDb = MapTrangThaiToDatabase(req.TrangThaiMoi);
                var ok = await _hoaDonService.UpdateTrangThaiAsync(req.HoaDonId, newDb, null);
                if (!ok) return Json(new { success = false, message = "Cập nhật thất bại (service)" });

                return Json(new { success = true, message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Hủy đơn (kèm lý do)
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> HuyDonLight([FromBody] HuyDonRequest req)
        {
            try
            {
                if (req == null) return Json(new { success = false, message = "Body rỗng" });
                var hoaDon = await _hoaDonService.GetByIdAsync(req.HoaDonId);
                if (hoaDon == null) return Json(new { success = false, message = "Không tìm thấy hóa đơn" });

                var ok = await _hoaDonService.UpdateTrangThaiAsync(req.HoaDonId, "Huy_Don", req.LyDoHuy ?? "");
                if (!ok) return Json(new { success = false, message = "Hủy đơn thất bại (service)" });

                return Json(new { success = true, message = "Đã hủy đơn thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Helpers
        private static bool IsTatCa(string s)
            => string.IsNullOrWhiteSpace(s) || s.Trim().Equals("TẤT CẢ", StringComparison.OrdinalIgnoreCase) || s.Trim().Equals("Tất cả", StringComparison.OrdinalIgnoreCase);

        // Map từ text UI -> giá trị DB
        private string MapTrangThaiToDatabase(string hienThi) => hienThi switch
        {
            "Chờ xác nhận" or "Đang xử lý" => "Chua_Xac_Nhan",
            "Chờ giao" => "Da_Xac_Nhan",
            "Vận chuyển" => "Dang_Giao_Hang",
            "Hoàn thành" or "Đã giao" or "Đã thanh toán" or "Hoàn tất" => "Hoan_Thanh",
            "Đã huỷ" => "Huy_Don",
            _ => string.IsNullOrWhiteSpace(hienThi) ? "" : hienThi
        };

        // Map từ DB -> text UI + badge
        private string MapTrangThaiFromDatabase(string db) => db switch
        {
            "Chua_Xac_Nhan" or "Dang_Xu_Ly" => "Chờ xác nhận",
            "Da_Xac_Nhan" => "Chờ giao",
            "Dang_Giao_Hang" => "Vận chuyển",
            "Hoan_Thanh" => "Hoàn thành",
            "Huy_Don" => "Đã huỷ",
            _ => db
        };
    }

    // DTOs
    public class CapNhatTrangThaiRequest
    {
        public int HoaDonId { get; set; }
        public string TrangThaiMoi { get; set; } = "";
    }
    public class HuyDonRequest
    {
        public int HoaDonId { get; set; }
        public string? LyDoHuy { get; set; }
    }

    // ViewModels
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
