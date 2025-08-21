using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.models;
using Service.IService;
using System.Globalization;
using System.Reflection;

namespace FE.Controllers
{
    public class BanHangTaiQuayController : Controller
    {
        private readonly ISanPhamService _sanPhamService;
        private readonly IHoaDonService _hoaDonService;

        public BanHangTaiQuayController(ISanPhamService sanPhamService, IHoaDonService hoaDonService)
        {
            _sanPhamService = sanPhamService;
            _hoaDonService = hoaDonService;
        }

        [HttpGet]
        public IActionResult Index() => View();

        // =========== PRODUCTS ===========
        // 1) GetSanPham: có fallback + thông báo lỗi gọn
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetSanPham(string tuKhoa = "", int page = 1, int pageSize = 12)
        {
            try
            {
                var all = (await _sanPhamService.GetAllAsync())?.ToList() ?? new List<SanPham>();

                if (!string.IsNullOrWhiteSpace(tuKhoa))
                {
                    all = all.Where(x =>
                        ContainsSafe(GetStr(x, "Ten_San_Pham", "Ten", "Name"), tuKhoa) ||
                        ContainsSafe(GetStr(x, "Ma_San_Pham", "MaSP", "Code", "SKU"), tuKhoa)
                    ).ToList();
                }

                var total = all.Count;
                var items = all
                    .OrderBy(x => GetStr(x, "Ten_San_Pham", "Ten", "Name"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new
                    {
                        ID_San_Pham = GetInt(x, "ID_San_Pham", "SanPhamId", "Id"),
                        Ma_San_Pham = GetStr(x, "Ma_San_Pham", "MaSP", "Code", "SKU") ?? "",
                        Ten_San_Pham = GetStr(x, "Ten_San_Pham", "Ten", "Name") ?? "",
                        Gia_Ban = GetDecimal(x, "Gia_Ban", "Gia", "Don_Gia", "Price") ?? 0m,
                        Hinh = GetStr(x, "Hinh_Anh_Url", "HinhAnh", "ImageUrl", "Img") ?? ""
                    })
                    .ToList();

                return Json(new { success = true, data = items, total, page, pageSize });
            }
            catch (Exception ex)
            {
                // Fallback: trả danh sách mẫu để UI không vỡ, đồng thời đính kèm message lỗi để bạn biết.
                var demo = FallbackProducts();
                return Json(new
                {
                    success = true,
                    data = demo
                        .Where(p => string.IsNullOrWhiteSpace(tuKhoa)
                                 || p.Ten_San_Pham.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                                 || p.Ma_San_Pham.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList(),
                    total = demo.Count,
                    page,
                    pageSize,
                    warn = "BE trả 500 – đang hiển thị dữ liệu tạm: " + ex.Message
                });
            }
        }

        // 2) Healthcheck: dùng để xem lỗi gốc từ BE
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> SanPhamHealth()
        {
            try
            {
                var rs = await _sanPhamService.GetAllAsync();
                var cnt = rs?.Count() ?? 0;
                return Json(new { ok = true, count = cnt });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message, stack = ex.StackTrace });
            }
        }

        // ===== Helper fallback & reflection helpers (đặt cùng controller) =====
        private List<dynamic> FallbackProducts() => new()
{
    new { ID_San_Pham = 1, Ma_San_Pham = "TRASUA01", Ten_San_Pham = "Trà sữa trân châu", Gia_Ban = 35000m, Hinh = "" },
    new { ID_San_Pham = 2, Ma_San_Pham = "TRASUA02", Ten_San_Pham = "Trà sữa đường đen", Gia_Ban = 39000m, Hinh = "" },
    new { ID_San_Pham = 3, Ma_San_Pham = "TRASUA03", Ten_San_Pham = "Hồng trà sữa",      Gia_Ban = 32000m, Hinh = "" },
    new { ID_San_Pham = 4, Ma_San_Pham = "TRASUA04", Ten_San_Pham = "Matcha sữa",        Gia_Ban = 42000m, Hinh = "" },
};

        // reflection helpers đã có ở bản trước (GetStr/GetInt/GetDecimal/ContainsSafe)

        // =========== OPTIONS (tạm tĩnh để khỏi phụ thuộc service) ===========
        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetOptions()
        {
            var doNgots = new[]
            {
                new { ID_DoNgot = 1, Muc_Do = "0%" },
                new { ID_DoNgot = 2, Muc_Do = "50%" },
                new { ID_DoNgot = 3, Muc_Do = "70%" },
                new { ID_DoNgot = 4, Muc_Do = "100%" }
            };
            var luongDas = new[]
            {
                new { ID_LuongDa = 1, Muc_Da = "Không đá" },
                new { ID_LuongDa = 2, Muc_Da = "Ít đá" },
                new { ID_LuongDa = 3, Muc_Da = "Vừa" },
                new { ID_LuongDa = 4, Muc_Da = "Nhiều" }
            };
            var toppings = new[]
            {
                new { ID_Topping = 1, Ten_Topping = "Trân châu", Gia_Topping = 5000m },
                new { ID_Topping = 2, Ten_Topping = "Thạch",     Gia_Topping = 5000m },
                new { ID_Topping = 3, Ten_Topping = "Pudding",   Gia_Topping = 7000m }
            };
            return Json(new { success = true, doNgots, luongDas, toppings });
        }

        // =========== VOUCHER (tạm: rule nội bộ, chặn >50%) ===========
        [HttpGet]
        [Produces("application/json")]
        public IActionResult KiemTraVoucher(string ma, decimal tongTien)
        {
            if (string.IsNullOrWhiteSpace(ma))
                return Json(new { success = false, message = "Chưa nhập mã voucher." });

            ma = ma.Trim().ToUpperInvariant();
            decimal giam = 0;

            // Demo: SALE10 (10%), SALE20 (20%), SALE50 (50%), SALE100 (100k)
            if (ma.StartsWith("SALE") && ma.Length >= 6 && decimal.TryParse(ma.Substring(4), out var pct))
            {
                if (pct <= 100) giam = tongTien * (pct / 100m);
            }
            else if (ma == "SALE100")
            {
                giam = 100_000m;
            }
            else
            {
                return Json(new { success = false, message = "Voucher không hợp lệ." });
            }

            // chặn >50%
            var max = tongTien * 0.5m;
            if (giam > max) giam = Math.Floor(max);

            var thanhToan = Math.Max(0, tongTien - giam);
            return Json(new { success = true, giam, thanhToan, voucher = new { ID_Voucher = 0, Ma_Voucher = ma } });
        }

        // =========== TẠO HÓA ĐƠN (gọi AddAsync có sẵn) ===========
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> TaoHoaDonTaiQuay([FromBody] TaoHoaDonTaiQuayRequest req)
        {
            try
            {
                if (req == null || req.Items == null || !req.Items.Any())
                    return Json(new { success = false, message = "Giỏ hàng rỗng." });

                if (req.TongTien > 0 && req.TienGiam > req.TongTien * 0.5m)
                    return Json(new { success = false, message = "Tổng giảm không vượt quá 50%." });

                var hoaDon = new HoaDon
                {
                    Ma_Hoa_Don = string.IsNullOrWhiteSpace(req.MaHoaDon) ? ("POS" + DateTime.Now.ToString("yyMMddHHmmss")) : req.MaHoaDon,
                    Ngay_Tao = DateTime.Now,
                    Trang_Thai = "Chua_Xac_Nhan",
                    Tong_Tien = Math.Max(0, req.TongTien - req.TienGiam),
                    Ghi_Chu = req.GhiChu,
                    // Gắn thêm các field bạn có trong model HoaDon, ví dụ:
                    // HinhThucThanhToanId / HinhThucThanhToan = ...
                    HoaDonChiTiets = req.Items.Select(i => new HoaDonChiTiet
                    {
                        ID_San_Pham = i.SanPhamId,
                        So_Luong = i.SoLuong,
                        Gia_San_Pham = i.DonGia
                    }).ToList()
                };

                await _hoaDonService.AddAsync(hoaDon);
                return Json(new { success = true, message = "Tạo hóa đơn thành công.", data = new { hoaDon.Ma_Hoa_Don } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ===== Helpers: reflection safe getters =====
        private static string? GetStr(object obj, params string[] names) => (string?)GetProp(obj, names);
        private static int GetInt(object obj, params string[] names) => (int)(GetProp(obj, names) ?? 0);
        private static decimal? GetDecimal(object obj, params string[] names)
        {
            var v = GetProp(obj, names);
            if (v == null) return null;
            if (v is decimal d) return d;
            if (v is double db) return (decimal)db;
            if (v is float f) return (decimal)f;
            if (decimal.TryParse(Convert.ToString(v, CultureInfo.InvariantCulture), out var p)) return p;
            return null;
        }
        private static object? GetProp(object obj, params string[] names)
        {
            var t = obj.GetType();
            foreach (var n in names)
            {
                var p = t.GetProperty(n, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null) return p.GetValue(obj);
            }
            return null;
        }
        private static bool ContainsSafe(string? src, string keyword)
            => (src ?? "").IndexOf(keyword ?? "", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    // ===== DTOs (giữ nguyên) =====
    public class TaoHoaDonTaiQuayRequest
    {
        public string? MaHoaDon { get; set; }
        public string HinhThucThanhToan { get; set; } = "TienMat";
        public string? KhachHang_SDT { get; set; }
        public string? GhiChu { get; set; }
        public int? VoucherId { get; set; }
        public decimal TongTien { get; set; }
        public decimal TienGiam { get; set; }
        public List<TaoHoaDonTaiQuayItem> Items { get; set; } = new();
    }

    public class TaoHoaDonTaiQuayItem
    {
        public int SanPhamId { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public int? DoNgotId { get; set; }
        public int? LuongDaId { get; set; }
        public List<int>? ToppingIds { get; set; }
    }
}
