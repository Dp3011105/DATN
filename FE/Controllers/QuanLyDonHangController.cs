using Microsoft.AspNetCore.Mvc;
using Service.IService;
using BE.models;

namespace FE.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        private readonly IHoaDonService _hoaDonService;

        public QuanLyDonHangController(IHoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }

        public async Task<IActionResult> Index(string tuKhoa = "", string trangThai = "")
        {
            var viewModel = new QuanLyDonHangViewModel();
            try
            {
                var hoaDonList = (await _hoaDonService.GetAllAsync())?.ToList() ?? new();

                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    hoaDonList = hoaDonList.Where(hd =>
                        (hd.KhachHang?.Ho_Ten?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) == true) ||
                        (hd.Ma_Hoa_Don?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) == true)).ToList();
                }

                if (!string.IsNullOrEmpty(trangThai) && trangThai != "TẤT CẢ")
                {
                    var mapped = MapTrangThaiToDatabase(trangThai);
                    hoaDonList = hoaDonList.Where(hd => hd.Trang_Thai == mapped).ToList();
                }

                viewModel.DanhSachHoaDon = hoaDonList;
            }
            catch { }

            viewModel.TuKhoa = tuKhoa;
            viewModel.TrangThai = trangThai;
            viewModel.TrangThaiList = new List<string>
            {
                "TẤT CẢ", "Chờ xác nhận", "Chờ giao", "Vận chuyển", "Đã giao", "Đã thanh toán", "Đã huỷ", "Hoàn thành"
            };

            return View(viewModel);
        }

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

        [HttpPost]
        public async Task<IActionResult> CapNhatTrangThai(int hoaDonId, string trangThaiMoi)
        {
            var hoaDon = await _hoaDonService.GetByIdAsync(hoaDonId);
            if (hoaDon == null) return Json(new { success = false, message = "Không tìm thấy hóa đơn" });

            hoaDon.Trang_Thai = MapTrangThaiToDatabase(trangThaiMoi);
            await _hoaDonService.UpdateAsync(hoaDonId, hoaDon);
            return Json(new { success = true, message = "Cập nhật thành công" });
        }

        [HttpPost]
        public async Task<IActionResult> HuyDon(int hoaDonId, string lyDoHuy = "")
        {
            var hoaDon = await _hoaDonService.GetByIdAsync(hoaDonId);
            if (hoaDon == null) return Json(new { success = false, message = "Không tìm thấy hóa đơn" });

            hoaDon.Trang_Thai = "Huy_Don";
            hoaDon.LyDoHuyDon = lyDoHuy;
            await _hoaDonService.UpdateAsync(hoaDonId, hoaDon);
            return Json(new { success = true, message = "Đã hủy đơn thành công" });
        }

        [HttpGet]
        public async Task<IActionResult> GetDanhSach(string tuKhoa = "", string trangThai = "")
        {
            try
            {
                var hoaDons = await _hoaDonService.GetAllAsync();
                List<object> list;

                if (hoaDons != null)
                {
                    list = hoaDons
                        .Where(hd =>
                            (string.IsNullOrEmpty(tuKhoa) ||
                                hd.KhachHang?.Ho_Ten?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) == true ||
                                hd.Ma_Hoa_Don?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) == true) &&
                            (string.IsNullOrEmpty(trangThai) || trangThai == "TẤT CẢ" ||
                                hd.Trang_Thai == MapTrangThaiToDatabase(trangThai))
                        )
                        .Select(hd => new
                        {
                            hd.ID_Hoa_Don,
                            hd.Ma_Hoa_Don,
                            hd.Tong_Tien,
                            KhachHang = hd.KhachHang?.Ho_Ten ?? "Khách lẻ",
                            Ngay_Tao = hd.Ngay_Tao.ToString("dd/MM/yyyy HH:mm"),
                            HinhThucThanhToan = hd.HinhThucThanhToan?.Phuong_Thuc_Thanh_Toan ?? "",
                            hd.Trang_Thai,
                            TrangThaiHienThi = MapTrangThaiFromDatabase(hd.Trang_Thai)
                        })
                        .Cast<object>() // ép về object để khớp kiểu
                        .ToList();
                }
                else
                {
                    list = new List<object>();
                }

                return Json(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string MapTrangThaiToDatabase(string hienThi) => hienThi switch
        {
            "Chờ xác nhận" => "Chua_Xac_Nhan",
            "Chờ giao" => "Da_Xac_Nhan",
            "Vận chuyển" => "Dang_Giao_Hang",
            "Đã giao" or "Đã thanh toán" or "Hoàn thành" => "Hoan_Thanh",
            "Đã huỷ" => "Huy_Don",
            _ => hienThi
        };

        private string MapTrangThaiFromDatabase(string db) => db switch
        {
            "Chua_Xac_Nhan" => "Chờ xác nhận",
            "Da_Xac_Nhan" => "Chờ giao",
            "Dang_Giao_Hang" => "Vận chuyển",
            "Hoan_Thanh" => "Hoàn thành",
            "Huy_Don" => "Đã huỷ",
            _ => db
        };
    }

    public class QuanLyDonHangViewModel
    {
        public List<HoaDon> DanhSachHoaDon { get; set; } = new();
        public string TuKhoa { get; set; } = "";
        public string TrangThai { get; set; } = "";
        public List<string> TrangThaiList { get; set; } = new();
    }

    public class ChiTietHoaDonViewModel
    {
        public HoaDon HoaDon { get; set; }
        public List<HoaDonChiTiet> ChiTiets { get; set; } = new();
    }
}
