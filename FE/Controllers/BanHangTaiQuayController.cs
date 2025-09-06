using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FE.Models;                 // SanPham, DoNgot, LuongDa, Topping
using FE.Service.IService;       // IProductService
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class BanHangTaiQuayController : Controller
    {
        // ===== CONFIG =====
        private const string BeBaseUrl = "https://localhost:7169";

        private static readonly string[] HoaDonEndpoints =
        {
            "/api/HoaDon",
            "/api/HoaDon/create",
            "/api/hoa-don",
            "/api/hoa-don/create"
        };

        private const string AdjustStockEndpoint = "/api/SanPham/tru-ton";

        // IDs mặc định (đổi theo hệ thống của bạn)
        private const int DEFAULT_KHACH_LE_ID = 1;
        private const int DEFAULT_NHAN_VIEN_ID = 1;
        private const int DEFAULT_SIZE_ID = 1;
        private const int DEFAULT_DONNGOT_ID = 1;
        private const int DEFAULT_LUONGDA_ID = 1;

        private readonly IProductService _productService;
        public BanHangTaiQuayController(IProductService productService) => _productService = productService;

        // ===== VIEW =====
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // >>> QUAN TRỌNG: TRẢ THẲNG SẢN PHẨM KHÔNG PROJECT
            var products = await _productService.GetAllProductsAsync() ?? new List<SanPham>();

            ViewBag.Products = products; // giữ nguyên KhuyenMais, Ma_San_Pham, v.v…
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync() ?? new List<DoNgot>();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync() ?? new List<LuongDa>();
            ViewBag.Toppings = await _productService.GetToppingsAsync() ?? new List<Topping>();
            return View();
        }

        // ===== API: SẢN PHẨM (có thể dùng cho lazy load/search) =====
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetSanPham(string tuKhoa = "", int page = 1, int pageSize = 12)
        {
            try
            {
                var all = await _productService.GetAllProductsAsync() ?? new List<SanPham>();

                if (!string.IsNullOrWhiteSpace(tuKhoa))
                {
                    var kw = tuKhoa.Trim();
                    all = all.Where(sp =>
                        (sp.Ten_San_Pham ?? "").Contains(kw, StringComparison.OrdinalIgnoreCase)
                      
                    ).ToList();
                }

                var total = all.Count;

                // Trả về đủ các field view có thể cần (đặc biệt KHÔNG bỏ KhuyenMais)
                var items = all
                    .OrderBy(sp => sp.Ten_San_Pham)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(sp => new
                    {
                        sp.ID_San_Pham,
                        sp.Ten_San_Pham,
                     
                        sp.Gia,
                        sp.Hinh_Anh,
                        sp.Mo_Ta,
                        sp.Trang_Thai,
                        SoLuongTon = sp.So_Luong,
                        sp.KhuyenMais // giữ nguyên để FE tính khuyến mãi
                    })
                    .ToList();

                return Json(new { success = true, data = items, total, page, pageSize });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ===== API: OPTIONS =====
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetOptions()
        {
            try
            {
                var doNgots = await _productService.GetDoNgotsAsync() ?? new List<DoNgot>();
                var luongDas = await _productService.GetLuongDasAsync() ?? new List<LuongDa>();
                var toppings = await _productService.GetToppingsAsync() ?? new List<Topping>();

                return Json(new
                {
                    success = true,
                    doNgots = doNgots.Select(d => new { d.ID_DoNgot, d.Muc_Do }),
                    luongDas = luongDas.Select(l => new { l.ID_LuongDa, Ten_LuongDa = l.Ten_LuongDa, Muc_Da = l.Ten_LuongDa }),
                    toppings = toppings.Select(t => new { t.ID_Topping, t.Ten, Gia = t.Gia, t.Hinh_Anh })
                });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // ===== API: VOUCHER DEMO =====
        [HttpGet]
        [Produces("application/json")]
        public IActionResult KiemTraVoucher(string ma, decimal tongTien)
        {
            if (string.IsNullOrWhiteSpace(ma))
                return Json(new { success = false, message = "Chưa nhập mã voucher." });

            ma = ma.Trim().ToUpperInvariant();
            decimal giam = 0;

            if (ma.StartsWith("SALE") && ma.Length >= 6 && decimal.TryParse(ma[4..], out var pct) && pct <= 100)
                giam = tongTien * (pct / 100m);
            else if (ma == "SALE100")
                giam = 100_000m;
            else
                return Json(new { success = false, message = "Voucher không hợp lệ." });

            var max = tongTien * 0.5m;
            if (giam > max) giam = Math.Floor(max);

            return Json(new
            {
                success = true,
                giam,
                thanhToan = Math.Max(0, tongTien - giam),
                voucher = new { ID_Voucher = 0, Ma_Voucher = ma }
            });
        }

        // ===== API: TẠO HÓA ĐƠN + TRỪ TỒN =====
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> TaoHoaDonTaiQuay([FromBody] TaoHoaDonTaiQuayRequest req)
        {
            try
            {
                if (req?.Items == null || !req.Items.Any())
                    return Json(new { success = false, message = "Giỏ hàng rỗng." });

                if (req.TongTien > 0 && req.TienGiam > req.TongTien * 0.5m)
                    return Json(new { success = false, message = "Tổng giảm không vượt quá 50%." });

                // ====== Kiểm tra tồn kho (server-side) ======
                var allProducts = await _productService.GetAllProductsAsync() ?? new List<SanPham>();
                var byId = allProducts.ToDictionary(p => p.ID_San_Pham, p => p);
                var overList = new List<string>();

                foreach (var it in req.Items)
                {
                    if (!byId.TryGetValue(it.SanPhamId, out var sp))
                    {
                        overList.Add($"SP#{it.SanPhamId} không tồn tại.");
                        continue;
                    }
                    if (it.SoLuong > sp.So_Luong)
                    {
                        var name = sp.Ten_San_Pham ?? $"SP#{sp.ID_San_Pham}";
                        overList.Add($"\"{name}\": đặt {it.SoLuong} > tồn {sp.So_Luong}");
                    }
                }

                if (overList.Count > 0)
                    return Json(new { success = false, message = "Vượt tồn kho:\n" + string.Join("\n", overList) });

                // ====== Build hoá đơn cho BE ======
                var now = DateTime.Now;
                var ma = string.IsNullOrWhiteSpace(req.MaHoaDon) ? ("POS" + now.ToString("yyMMddHHmmss")) : req.MaHoaDon;

                int ptttId = req.HinhThucThanhToanId ?? 1;

                var loai = (req.LoaiHoaDon ?? "TaiQuay").Trim();
                var allow = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "TaiQuay", "Online", "GiaoHang" };
                if (!allow.Contains(loai)) loai = "TaiQuay";

                string trangThai = loai.Equals("TaiQuay", StringComparison.OrdinalIgnoreCase) ? "Hoan_Thanh" : "Chua_Xac_Nhan";

                var chiTiets = req.Items.Select((it, idx) => new
                {
                    ID_HoaDon_ChiTiet = 0,
                    ID_San_Pham = it.SanPhamId,
                    ID_Size = DEFAULT_SIZE_ID,
                    ID_SanPham_DoNgot = it.DoNgotId ?? DEFAULT_DONNGOT_ID,
                    ID_LuongDa = it.LuongDaId ?? DEFAULT_LUONGDA_ID,
                    Ma_HoaDon_ChiTiet = $"{ma}-{idx + 1}",
                    Gia_Them_Size = 0m,
                    Gia_San_Pham = it.DonGia,
                    So_Luong = it.SoLuong <= 0 ? 1 : it.SoLuong,
                    Tong_Tien = it.DonGia * (it.SoLuong <= 0 ? 1 : it.SoLuong),
                    Ghi_Chu = "",
                    Ngay_Tao = now
                }).ToList();

                var payload = new
                {
                    ID_Hoa_Don = 0,
                    ID_Khach_Hang = DEFAULT_KHACH_LE_ID,
                    ID_Nhan_Vien = DEFAULT_NHAN_VIEN_ID,
                    ID_Hinh_Thuc_Thanh_Toan = ptttId,
                    ID_Dia_Chi = (int?)null,
                    ID_Phi_Ship = (int?)null,

                    Dia_Chi_Tu_Nhap = loai.Equals("GiaoHang", StringComparison.OrdinalIgnoreCase) ? (req.DiaChiTuNhap ?? "") : "",
                    Ngay_Tao = now,
                    Tong_Tien = Math.Max(0, req.TongTien - req.TienGiam),
                    Phi_Ship = 0m,
                    Trang_Thai = trangThai,
                    Ghi_Chu = req.GhiChu ?? "",
                    Ma_Hoa_Don = ma,
                    Loai_Hoa_Don = loai,

                    HoaDonChiTiets = chiTiets,
                    HoaDonVouchers = (req.VoucherId.HasValue && req.TienGiam > 0)
                        ? new[] { new { ID_HoaDonVoucher = 0, ID_Voucher = req.VoucherId.Value, Gia_Tri_Giam = req.TienGiam } }
                        : Array.Empty<object>(),
                    LichSuHoaDons = Array.Empty<object>()
                };

                // Nếu bạn gặp lỗi SSL self-signed khi gọi https://localhost:7169,
                // mở comment đoạn handler dưới đây:
                // var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true };
                // using var http = new HttpClient(handler) { BaseAddress = new Uri(BeBaseUrl), Timeout = TimeSpan.FromSeconds(25) };

                using var http = new HttpClient { BaseAddress = new Uri(BeBaseUrl), Timeout = TimeSpan.FromSeconds(25) };
                HttpResponseMessage? resp = null;
                string bodyText = "";

                foreach (var ep in HoaDonEndpoints)
                {
                    try
                    {
                        resp = await http.PostAsJsonAsync(ep, payload);
                        bodyText = await resp.Content.ReadAsStringAsync();
                        if (resp.StatusCode == HttpStatusCode.NotFound || resp.StatusCode == HttpStatusCode.MethodNotAllowed) continue;
                        break;
                    }
                    catch (Exception exTry) { bodyText = exTry.Message; }
                }

                if (resp == null)
                    return Json(new { success = false, message = "Không kết nối được BE." });
                if (!resp.IsSuccessStatusCode)
                    return Json(new { success = false, message = $"BE {(int)resp.StatusCode}: {bodyText}" });

                // ====== TRỪ TỒN KHO SAU KHI TẠO HÓA ĐƠN ======
                try
                {
                    var adjustBody = req.Items.Select(it => new { ID_San_Pham = it.SanPhamId, SoLuongTru = it.SoLuong }).ToList();
                    var stockResp = await http.PostAsJsonAsync(AdjustStockEndpoint, adjustBody);
                    var stockRespText = await stockResp.Content.ReadAsStringAsync();

                    if (!stockResp.IsSuccessStatusCode)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Tạo hoá đơn thành công, nhưng trừ tồn kho thất bại.",
                            be = new { status = (int)resp.StatusCode, createBody = bodyText, adjustStatus = (int)stockResp.StatusCode, adjustBody = stockRespText }
                        });
                    }
                }
                catch (Exception exAdj)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Tạo hoá đơn thành công, nhưng gặp lỗi khi trừ tồn kho: " + exAdj.Message
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Tạo hoá đơn & trừ tồn thành công."
                });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }
    }

    // ===== DTO FE =====
    public class TaoHoaDonTaiQuayRequest
    {
        public string? MaHoaDon { get; set; }
        public int? HinhThucThanhToanId { get; set; }
        public string? LoaiHoaDon { get; set; }        // "TaiQuay" | "Online" | "GiaoHang"
        public string? KhachHang_SDT { get; set; }
        public string? GhiChu { get; set; }
        public int? VoucherId { get; set; }
        public decimal TongTien { get; set; }
        public decimal TienGiam { get; set; }
        public string? DiaChiTuNhap { get; set; }
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
