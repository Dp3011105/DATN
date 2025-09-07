using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FE.Models;                 // SanPham, DoNgot, LuongDa, Topping
using FE.Service.IService;       // IProductService
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    //[Authorize] // tuỳ bạn, nếu chưa login thì có thể bỏ
    [AutoValidateAntiforgeryToken] // bật anti-forgery mặc định cho POST
    public class BanHangTaiQuayController : Controller
    {
        // ===== CONFIG =====
        private const string BeBaseUrl = "https://localhost:7169";

        // Tạo hoá đơn: thử theo thứ tự cho hợp BE hiện tại
        private static readonly string[] HoaDonCreateEndpoints =
        {
            "/api/HoaDon",
            "/api/HoaDon/create",
            "/api/hoa-don",
            "/api/hoa-don/create"
        };

        // Trừ tồn: nếu BE bạn có 1 endpoint chuẩn – set lại name/path này
        private const string AdjustStockEndpoint = "/api/SanPham/tru-ton";

        // IDs mặc định
        private const int DEFAULT_KHACH_LE_ID = 1;
        private const int DEFAULT_NHAN_VIEN_ID = 1;
        private const int DEFAULT_SIZE_ID = 1;
        private const int DEFAULT_DONNGOT_ID = 1;
        private const int DEFAULT_LUONGDA_ID = 1;

        private readonly IProductService _productService;
        public BanHangTaiQuayController(IProductService productService) => _productService = productService;

        // ===== VIEW =====
        [HttpGet]
        [AllowAnonymous] // nếu bán tại quầy không yêu cầu login thì để AllowAnonymous
        public async Task<IActionResult> Index()
        {
            // TRẢ THẲNG model từ BE để FE có đầy đủ KhuyenMais, So_Luong...
            var products = await _productService.GetAllProductsAsync() ?? new List<SanPham>();
            ViewBag.Products = products;

            ViewBag.DoNgots = await _productService.GetDoNgotsAsync() ?? new List<DoNgot>();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync() ?? new List<LuongDa>();
            ViewBag.Toppings = await _productService.GetToppingsAsync() ?? new List<Topping>();
            return View();
        }

        // ===== API: Lấy sản phẩm (optional nếu cần server-paging) =====
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
                        sp.KhuyenMais
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

        // ===== Helper: Tính giá sau KM (Server-side) =====
        private static DateTime? ParseAnyDate(object? v)
        {
            if (v == null) return null;
            if (v is DateTime dt) return dt;
            var s = v.ToString();
            if (string.IsNullOrWhiteSpace(s)) return null;

            // /Date(1715731200000)/
            var m = System.Text.RegularExpressions.Regex.Match(s, @"\/Date\((\d+)\)\/");
            if (m.Success && long.TryParse(m.Groups[1].Value, out var ms))
                return DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;

            // dd/MM/yyyy [HH:mm:ss]
            var m2 = System.Text.RegularExpressions.Regex.Match(s, @"^(\d{1,2})[\/-](\d{1,2})[\/-](\d{4})(?:[ T](\d{1,2}):(\d{1,2})(?::(\d{1,2}))?)?$");
            if (m2.Success)
            {
                var dd = int.Parse(m2.Groups[1].Value);
                var MM = int.Parse(m2.Groups[2].Value);
                var yyyy = int.Parse(m2.Groups[3].Value);
                var HH = string.IsNullOrEmpty(m2.Groups[4].Value) ? 0 : int.Parse(m2.Groups[4].Value);
                var mm = string.IsNullOrEmpty(m2.Groups[5].Value) ? 0 : int.Parse(m2.Groups[5].Value);
                var ss = string.IsNullOrEmpty(m2.Groups[6].Value) ? 0 : int.Parse(m2.Groups[6].Value);
                return new DateTime(yyyy, MM, dd, HH, mm, ss, DateTimeKind.Local).ToUniversalTime();
            }

            // ISO
            if (DateTime.TryParse(s, out var iso))
                return DateTime.SpecifyKind(iso, DateTimeKind.Local).ToUniversalTime();

            return null;
        }

        private static decimal CalcPriceAfterPromo(SanPham sp, DateTime utcNow)
        {
            var origin = sp?.Gia ?? 0m;
            if (sp?.KhuyenMais == null || sp.KhuyenMais.Count == 0) return origin;

            // Chọn KM có hiệu lực → lấy giá nhỏ nhất
            decimal best = origin;
            foreach (var km in sp.KhuyenMais)
            {
                var s = ParseAnyDate(km?.Ngay_Bat_Dau);
                var e = ParseAnyDate(km?.Ngay_Ket_Thuc);
                if (!s.HasValue || !e.HasValue) continue;
                if (utcNow >= s.Value && utcNow <= e.Value)
                {
                    var promoPrice = km?.Gia_Giam ?? origin; // bạn đang dùng Gia_Giam là GIÁ CUỐI
                    if (promoPrice < best) best = promoPrice;
                }
            }
            return best;
        }

        // ===== API: TẠO HÓA ĐƠN – Recalc server-side + check tồn + gọi BE =====
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> TaoHoaDonTaiQuay([FromBody] TaoHoaDonTaiQuayRequest req)
        {
            try
            {
                if (req?.Items == null || !req.Items.Any())
                    return Json(new { success = false, message = "Giỏ hàng rỗng." });

                var allProducts = await _productService.GetAllProductsAsync() ?? new List<SanPham>();
                var toppingsAll = await _productService.GetToppingsAsync() ?? new List<Topping>();

                var prodMap = allProducts.ToDictionary(p => p.ID_San_Pham, p => p);
                var toppingMap = toppingsAll.ToDictionary(t => t.ID_Topping, t => t);

                // === Kiểm tồn & tính tiền server-side ===
                var overList = new List<string>();
                var utcNow = DateTime.UtcNow;
                decimal tong = 0m;

                var chiTietList = new List<object>(); // payload gửi BE
                int lineNo = 0;

                foreach (var it in req.Items)
                {
                    lineNo++;
                    if (!prodMap.TryGetValue(it.SanPhamId, out var sp))
                    {
                        overList.Add($"SP#{it.SanPhamId} không tồn tại.");
                        continue;
                    }

                    var sl = it.SoLuong <= 0 ? 1 : it.SoLuong;

                    // Check tồn: so với So_Luong của SP
                    var ton = sp.So_Luong;
                    if (sl > ton)
                    {
                        overList.Add($"\"{sp.Ten_San_Pham}\": đặt {sl} > tồn {ton}");
                    }

                    // Giá sau KM (giá SP cuối)
                    var giaSauKm = CalcPriceAfterPromo(sp, utcNow);

                    // Topping
                    decimal tienTopOne = 0m;
                    var topIds = (it.ToppingIds ?? new List<int>()).Distinct().ToList();
                    var topPayload = new List<object>();
                    foreach (var tid in topIds)
                    {
                        if (!toppingMap.TryGetValue(tid, out var top)) continue;
                        var g = top?.Gia ?? 0m;
                        tienTopOne += g;
                        topPayload.Add(new { ID_Topping = tid, Gia = g });
                    }

                    var donGiaOne = Math.Max(0, giaSauKm); // chưa cộng topping
                    tong += (donGiaOne + tienTopOne) * sl;

                    // Build chi tiết cho BE
                    chiTietList.Add(new
                    {
                        ID_HoaDon_ChiTiet = 0,
                        ID_San_Pham = it.SanPhamId,
                        ID_Size = DEFAULT_SIZE_ID,                       // TODO: nếu bạn có size theo giá riêng, map đúng vào đây
                        ID_SanPham_DoNgot = it.DoNgotId ?? DEFAULT_DONNGOT_ID,
                        ID_LuongDa = it.LuongDaId ?? DEFAULT_LUONGDA_ID,
                        Ma_HoaDon_ChiTiet = "",                          // để BE tự sinh
                        Gia_Them_Size = 0m,                               // TODO: cộng giá size nếu có
                        Gia_San_Pham = donGiaOne,                         // GIÁ SP SAU KM (chưa gồm topping)
                        So_Luong = sl,
                        Tong_Tien = (donGiaOne + tienTopOne) * sl,        // tổng dòng gồm topping
                        Ghi_Chu = "",
                        Ngay_Tao = DateTime.Now,
                        HoaDonChiTietToppings = topPayload                // gợi ý, tuỳ BE có đọc không
                    });
                }

                if (overList.Count > 0)
                    return Json(new { success = false, message = "Vượt tồn kho:\n" + string.Join("\n", overList) });

                // Validate giảm giá ≤ 50% tạm tính (server)
                var giam = Math.Max(0, Math.Min(req.TienGiam, tong * 0.5m));
                var thanhToan = Math.Max(0, tong - giam);

                // ===== Build hoá đơn cho BE =====
                var now = DateTime.Now;
                var ma = string.IsNullOrWhiteSpace(req.MaHoaDon) ? ("POS" + now.ToString("yyMMddHHmmss") + "-" + Guid.NewGuid().ToString("N").Substring(0, 4)) : req.MaHoaDon;

                var loai = (req.LoaiHoaDon ?? "TaiQuay").Trim();
                var allow = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "TaiQuay", "Online", "GiaoHang" };
                if (!allow.Contains(loai)) loai = "TaiQuay";

                string trangThai = loai.Equals("TaiQuay", StringComparison.OrdinalIgnoreCase) ? "Hoan_Thanh" : "Chua_Xac_Nhan";
                int ptttId = req.HinhThucThanhToanId ?? 1;

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
                    Tong_Tien = thanhToan,   // tổng cuối sau giảm
                    Phi_Ship = 0m,
                    Trang_Thai = trangThai,
                    Ghi_Chu = req.GhiChu ?? "",
                    Ma_Hoa_Don = ma,
                    Loai_Hoa_Don = loai,

                    HoaDonChiTiets = chiTietList,
                    HoaDonVouchers = (req.VoucherId.HasValue && giam > 0)
                        ? new[] { new { ID_HoaDonVoucher = 0, ID_Voucher = req.VoucherId.Value, Gia_Tri_Giam = giam } }
                        : Array.Empty<object>(),
                    LichSuHoaDons = Array.Empty<object>()
                };

                using var http = new HttpClient { BaseAddress = new Uri(BeBaseUrl), Timeout = TimeSpan.FromSeconds(25) };

                HttpResponseMessage? resp = null;
                string respText = "";
                string usedEndpoint = "";

                foreach (var ep in HoaDonCreateEndpoints)
                {
                    try
                    {
                        resp = await http.PostAsJsonAsync(ep, payload);
                        respText = await resp.Content.ReadAsStringAsync();
                        if (resp.StatusCode == HttpStatusCode.NotFound || resp.StatusCode == HttpStatusCode.MethodNotAllowed)
                        {
                            // thử endpoint khác
                            continue;
                        }
                        usedEndpoint = ep;
                        break;
                    }
                    catch (Exception exTry)
                    {
                        respText = exTry.Message;
                    }
                }

                if (resp == null)
                    return Json(new { success = false, message = "Không kết nối được BE." });
                if (!resp.IsSuccessStatusCode)
                    return Json(new { success = false, message = $"BE {(int)resp.StatusCode}: {respText}" });

                // Parse kết quả để lấy ID/Code nếu có
                string returnedCode = null;
                int? returnedId = null;
                try
                {
                    var beObj = System.Text.Json.JsonDocument.Parse(respText).RootElement;
                    if (beObj.TryGetProperty("ma_Hoa_Don", out var codeProp) && codeProp.ValueKind == System.Text.Json.JsonValueKind.String)
                        returnedCode = codeProp.GetString();
                    if (beObj.TryGetProperty("ma_HoaDon", out var codeProp2) && codeProp2.ValueKind == System.Text.Json.JsonValueKind.String)
                        returnedCode = codeProp2.GetString() ?? returnedCode;
                    if (beObj.TryGetProperty("id_Hoa_Don", out var idProp) && idProp.TryGetInt32(out var idVal))
                        returnedId = idVal;
                    if (returnedCode == null && beObj.TryGetProperty("ma_Hoa_Don", out var any) && any.ValueKind == System.Text.Json.JsonValueKind.Number)
                        returnedCode = any.GetRawText();
                }
                catch { /* ignore parse */ }

                // ===== TRỪ TỒN KHO SAU KHI TẠO HÓA ĐƠN (fallback nếu BE chưa gom transaction) =====
                try
                {
                    var adjustBody = req.Items.Select(it => new { ID_San_Pham = it.SanPhamId, SoLuongTru = Math.Max(1, it.SoLuong) }).ToList();
                    var stockResp = await http.PostAsJsonAsync(AdjustStockEndpoint, adjustBody);
                    if (!stockResp.IsSuccessStatusCode)
                    {
                        var stockText = await stockResp.Content.ReadAsStringAsync();
                        return Json(new
                        {
                            success = true,
                            message = "Tạo hoá đơn thành công, nhưng trừ tồn kho thất bại (hãy gộp transaction ở BE).",
                            be = new { endpoint = usedEndpoint, status = (int)resp.StatusCode },
                            adjust = new { status = (int)stockResp.StatusCode, body = stockText },
                            id = returnedId,
                            code = returnedCode
                        });
                    }
                }
                catch (Exception exAdj)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Tạo hoá đơn thành công, nhưng gặp lỗi khi trừ tồn kho (hãy gộp transaction ở BE): " + exAdj.Message,
                        id = returnedId,
                        code = returnedCode
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Tạo hoá đơn & trừ tồn thành công.",
                    id = returnedId,
                    code = returnedCode
                });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // ===== Tuỳ chọn: endpoint “trừ tồn sau thanh toán” cho FE call phụ (tránh 404) =====
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> TruTonKhoSauThanhToan([FromBody] List<AdjustStockRow> rows)
        {
            try
            {
                if (rows == null || rows.Count == 0)
                    return Json(new { success = false, message = "Thiếu dữ liệu trừ tồn." });

                using var http = new HttpClient { BaseAddress = new Uri(BeBaseUrl), Timeout = TimeSpan.FromSeconds(15) };
                var resp = await http.PostAsJsonAsync(AdjustStockEndpoint, rows.Select(r => new { ID_San_Pham = r.ID_San_Pham, SoLuongTru = r.SoLuongTru }));
                var tx = await resp.Content.ReadAsStringAsync();
                if (!resp.IsSuccessStatusCode) return Json(new { success = false, message = $"BE {(int)resp.StatusCode}: {tx}" });
                return Json(new { success = true });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }
    }

    // ===== DTOs =====
    public class TaoHoaDonTaiQuayRequest
    {
        public string? MaHoaDon { get; set; }
        public int? HinhThucThanhToanId { get; set; }
        public string? LoaiHoaDon { get; set; }        // "TaiQuay" | "Online" | "GiaoHang"
        public string? KhachHang_SDT { get; set; }
        public string? GhiChu { get; set; }
        public int? VoucherId { get; set; }
        public decimal TongTien { get; set; }          // sẽ bị IGNORE – server tự tính lại
        public decimal TienGiam { get; set; }          // server clamp ≤ 50%
        public string? DiaChiTuNhap { get; set; }
        public List<TaoHoaDonTaiQuayItem> Items { get; set; } = new();
    }

    public class TaoHoaDonTaiQuayItem
    {
        public int SanPhamId { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }            // client gửi nhưng server không tin
        public int? DoNgotId { get; set; }
        public int? LuongDaId { get; set; }
        public List<int>? ToppingIds { get; set; }
        public int? SizeId { get; set; }               // nếu bạn có size riêng
    }

    public class AdjustStockRow
    {
        public int ID_San_Pham { get; set; }
        public int SoLuongTru { get; set; }
    }
}
