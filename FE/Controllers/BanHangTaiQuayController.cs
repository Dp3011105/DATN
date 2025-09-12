using FE.Filters;
using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FE.Controllers
{
    [RoleAuthorize(2, 3)]
    [AutoValidateAntiforgeryToken]
    public class BanHangTaiQuayController : Controller
    {
        // ===== CONFIG =====
        private const string BeBaseUrl = "https://localhost:7169";

        private static readonly string[] HoaDonCreateEndpoints =
        {
            "/api/HoaDon",
            "/api/HoaDon/create",
            "/api/hoa-don",
            "/api/hoa-don/create"
        };

        // ⚠️ Endpoint này ở BE hiện có khả năng làm mất dữ liệu chi tiết -> tạm tắt bằng flag
        private const string AdjustStockEndpoint = "/api/SanPham/tru-ton";
        private const bool CALL_ADJUST_STOCK_AFTER_CREATE = false; // => đổi true khi BE đã sửa

        // Voucher BE routes
        private const string VoucherByCodeRoute = "/api/Voucher/code/{code}";
        private const string VoucherConsumeByIdRoute = "/api/Voucher/consume/{id}?qty=1";
        private const string VoucherConsumeByCodeRoute = "/api/Voucher/consume-by-code/{code}?qty=1";

        // IDs mặc định/fallback
        private const int DEFAULT_KHACH_LE_ID = 1;
        private const int DEFAULT_NHAN_VIEN_ID = 1;
        private const int DEFAULT_DONNGOT_ID = 1;
        private const int DEFAULT_LUONGDA_ID = 1;

        // Map SIZE giống UI
        private static readonly Dictionary<int, (string name, decimal extra)> SIZE_META = new()
        {
            {1, ("Cơ bản", 0m)},
            {2, ("Large", 10000m)},
            {3, ("X-Large", 15000m)}
        };

        // === Helper: mã chi tiết hoá đơn duy nhất (tránh trùng unique index) ===
        private static string NewChiTietCode(int lineNo)
        {
            var rand = Guid.NewGuid().ToString("N")[..4];
            return $"CT{DateTime.UtcNow:yyMMddHHmmss}{lineNo:D2}{rand}";
        }

        private readonly IProductService _productService;
        public BanHangTaiQuayController(IProductService productService) => _productService = productService;

        // ===== VIEW =====
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync() ?? new List<SanPham>();
            ViewBag.Products = products;
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync() ?? new List<DoNgot>();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync() ?? new List<LuongDa>();
            ViewBag.Toppings = await _productService.GetToppingsAsync() ?? new List<Topping>();
            return View();
        }

        // ===== OPTIONS (optional dùng cho client fetch) =====
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
                    toppings = toppings.Select(t => new { t.ID_Topping, Ten = t.Ten ?? t.Ten, Gia = t.Gia, t.Hinh_Anh })
                });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // ===== Helpers KM/Ngày =====
        private static DateTime? ParseAnyDate(object? v)
        {
            if (v == null) return null;
            if (v is DateTime dt) return dt;
            var s = v.ToString();
            if (string.IsNullOrWhiteSpace(s)) return null;

            var m = Regex.Match(s, @"\/Date\((\d+)\)\/");
            if (m.Success && long.TryParse(m.Groups[1].Value, out var ms))
                return DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;

            var m2 = Regex.Match(s, @"^(\d{1,2})[\/-](\d{1,2})[\/-](\d{4})(?:[ T](\d{1,2}):(\d{1,2})(?::(\d{1,2}))?)?$");
            if (m2.Success)
            {
                int dd = int.Parse(m2.Groups[1].Value);
                int MM = int.Parse(m2.Groups[2].Value);
                int yyyy = int.Parse(m2.Groups[3].Value);
                int HH = string.IsNullOrEmpty(m2.Groups[4].Value) ? 0 : int.Parse(m2.Groups[4].Value);
                int mm = string.IsNullOrEmpty(m2.Groups[5].Value) ? 0 : int.Parse(m2.Groups[5].Value);
                int ss = string.IsNullOrEmpty(m2.Groups[6].Value) ? 0 : int.Parse(m2.Groups[6].Value);
                return new DateTime(yyyy, MM, dd, HH, mm, ss, DateTimeKind.Local).ToUniversalTime();
            }

            if (DateTime.TryParse(s, out var iso))
                return DateTime.SpecifyKind(iso, DateTimeKind.Local).ToUniversalTime();

            return null;
        }

        private static decimal CalcPriceAfterPromo(SanPham sp, DateTime utcNow)
        {
            var origin = sp?.Gia ?? 0m;
            if (sp?.KhuyenMais == null || sp.KhuyenMais.Count == 0) return origin;

            decimal best = origin;
            foreach (var km in sp.KhuyenMais)
            {
                var s = ParseAnyDate(km?.Ngay_Bat_Dau);
                var e = ParseAnyDate(km?.Ngay_Ket_Thuc);
                if (!s.HasValue || !e.HasValue) continue;
                if (utcNow >= s.Value && utcNow <= e.Value)
                {
                    // Ở DB của bạn Gia_Giam là GIÁ SAU KM (không phải số tiền giảm).
                    var promoPrice = km?.Gia_Giam ?? origin;
                    if (promoPrice < best) best = promoPrice;
                }
            }
            return best;
        }

        // =====================================================================
        // ============== VOUCHER: API kiểm tra mã từ FE ========================
        // =====================================================================

        public class CheckVoucherRequest
        {
            public string Code { get; set; } = "";
            public decimal Subtotal { get; set; } // tạm tính hiện tại
        }

        public class CheckVoucherResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = "";
            public int? VoucherId { get; set; }
            public string? Code { get; set; }
            public decimal Discount { get; set; }   // số tiền giảm
            public decimal Percentage { get; set; } // % giảm thực tế
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> CheckVoucher([FromBody] CheckVoucherRequest req)
        {
            try
            {
                var code = (req?.Code ?? "").Trim();
                var subtotal = Math.Max(0m, req?.Subtotal ?? 0m);
                if (string.IsNullOrWhiteSpace(code))
                    return Json(new CheckVoucherResponse { Success = false, Message = "Thiếu mã voucher." });

                var endpoint = VoucherByCodeRoute.Replace("{code}", Uri.EscapeDataString(code));
                using var http = new HttpClient { BaseAddress = new Uri(BeBaseUrl), Timeout = TimeSpan.FromSeconds(15) };

                HttpResponseMessage resp;
                string tx;
                try
                {
                    resp = await http.GetAsync(endpoint);
                    tx = await resp.Content.ReadAsStringAsync();
                }
                catch (Exception exCall)
                {
                    return Json(new CheckVoucherResponse { Success = false, Message = "Không gọi được BE: " + exCall.Message });
                }

                if (!resp.IsSuccessStatusCode)
                    return Json(new CheckVoucherResponse { Success = false, Message = $"BE trả {(int)resp.StatusCode} khi tra voucher." });

                JsonElement root;
                try { root = JsonDocument.Parse(tx).RootElement; }
                catch (Exception exParse)
                {
                    return Json(new CheckVoucherResponse { Success = false, Message = "BE trả không phải JSON: " + exParse.Message });
                }

                if (root.ValueKind != JsonValueKind.Object)
                    return Json(new CheckVoucherResponse { Success = false, Message = "Dữ liệu voucher không đúng định dạng." });

                // Map field (camel/Pascal)
                string? codeFromObj =
                      root.TryGetProperty("ma_Voucher", out var pCode1) ? pCode1.GetString()
                    : root.TryGetProperty("Ma_Voucher", out var pCode2) ? pCode2.GetString()
                    : null;

                int? id =
                      (root.TryGetProperty("id_Voucher", out var pId1) && pId1.TryGetInt32(out var id1)) ? id1
                    : (root.TryGetProperty("ID_Voucher", out var pId2) && pId2.TryGetInt32(out var id2)) ? id2
                    : (int?)null;

                bool trangThai =
                    (root.TryGetProperty("trang_Thai", out var pSt1) && (
                        pSt1.ValueKind == JsonValueKind.True ||
                        (pSt1.ValueKind == JsonValueKind.Number && pSt1.TryGetInt32(out var stn1) && stn1 == 1)
                    )) ||
                    (root.TryGetProperty("Trang_Thai", out var pSt2) && (
                        pSt2.ValueKind == JsonValueKind.True ||
                        (pSt2.ValueKind == JsonValueKind.Number && pSt2.TryGetInt32(out var stn2) && stn2 == 1)
                    ));

                int soLuong =
                      (root.TryGetProperty("so_Luong", out var pSl1) && pSl1.TryGetInt32(out var sl1)) ? sl1
                    : (root.TryGetProperty("So_Luong", out var pSl2) && pSl2.TryGetInt32(out var sl2)) ? sl2
                    : 0;

                decimal giaTriGiam =
                      (root.TryGetProperty("gia_Tri_Giam", out var pG1) && pG1.TryGetDecimal(out var g1)) ? g1
                    : (root.TryGetProperty("Gia_Tri_Giam", out var pG2) && pG2.TryGetDecimal(out var g2)) ? g2
                    : 0m;

                decimal minOrder =
                      (root.TryGetProperty("so_Tien_Dat_Yeu_Cau", out var pMin1) && pMin1.TryGetDecimal(out var m1)) ? m1
                    : (root.TryGetProperty("So_Tien_Dat_Yeu_Cau", out var pMin2) && pMin2.TryGetDecimal(out var m2)) ? m2
                    : 0m;

                DateTime? start = ParseAnyDate(
                      root.TryGetProperty("ngay_Bat_Dau", out var pS1) ? pS1.ToString()
                    : root.TryGetProperty("Ngay_Bat_Dau", out var pS2) ? pS2.ToString()
                    : null);

                DateTime? end = ParseAnyDate(
                      root.TryGetProperty("ngay_Ket_Thuc", out var pE1) ? pE1.ToString()
                    : root.TryGetProperty("Ngay_Ket_Thuc", out var pE2) ? pE2.ToString()
                    : null);

                if (!trangThai) return Json(new CheckVoucherResponse { Success = false, Message = "Voucher đã tắt." });
                if (soLuong <= 0) return Json(new CheckVoucherResponse { Success = false, Message = "Voucher đã hết lượt dùng." });

                var nowUtc = DateTime.UtcNow;
                if (start.HasValue && nowUtc < start.Value) return Json(new CheckVoucherResponse { Success = false, Message = "Chưa đến thời gian áp dụng." });
                if (end.HasValue && nowUtc > end.Value) return Json(new CheckVoucherResponse { Success = false, Message = "Voucher đã hết hạn." });

                if (subtotal < minOrder)
                    return Json(new CheckVoucherResponse { Success = false, Message = $"Đơn tối thiểu {minOrder:n0}đ để dùng voucher." });

                // QUY ƯỚC: gia_Tri_Giam là % (0–100). Áp trần 50% đơn.
                var pct = Math.Clamp(giaTriGiam, 0m, 100m);
                var rawDiscount = subtotal * (pct / 100m);
                var cap = subtotal * 0.5m;
                var discount = Math.Min(rawDiscount, cap);

                if (discount <= 0)
                    return Json(new CheckVoucherResponse { Success = false, Message = "Voucher không mang lại giảm giá." });

                return Json(new CheckVoucherResponse
                {
                    Success = true,
                    Message = $"Áp dụng {pct}% (đã áp trần 50%).",
                    VoucherId = id,
                    Code = codeFromObj ?? code,
                    Discount = discount,
                    Percentage = pct
                });
            }
            catch (Exception ex)
            {
                return Json(new CheckVoucherResponse { Success = false, Message = ex.Message });
            }
        }

        // =====================================================================
        // ======================== TẠO HÓA ĐƠN =================================
        // =====================================================================
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

                var utcNow = DateTime.UtcNow;
                var overList = new List<string>();
                decimal tamTinh = 0m;

                var chiTietList = new List<object>();
                int lineNo = 0;

                foreach (var it in req.Items)
                {
                    lineNo++;

                    if (!prodMap.TryGetValue(it.SanPhamId, out var sp))
                    {
                        overList.Add($"SP#{it.SanPhamId} không tồn tại.");
                        continue;
                    }

                    // ===== GỌI CHI TIẾT SẢN PHẨM TỪ BE ĐỂ LỌC OPTION HỢP LỆ =====
                    var allowed = await GetAllowedOptionsFromSanPhamApi(it.SanPhamId);

                    var sizeId = (it.SizeId.HasValue && allowed.SizeIds.Contains(it.SizeId.Value)) ? it.SizeId.Value : 1;
                    var doNgotId = (it.DoNgotId.HasValue && allowed.DoNgotIds.Contains(it.DoNgotId.Value)) ? it.DoNgotId.Value : DEFAULT_DONNGOT_ID;
                    var luongDaId = (it.LuongDaId.HasValue && allowed.LuongDaIds.Contains(it.LuongDaId.Value)) ? it.LuongDaId.Value : DEFAULT_LUONGDA_ID;

                    var topIds = (it.ToppingIds ?? new List<int>()).Distinct().Where(id => allowed.ToppingIds.Contains(id)).ToList();

                    var sl = it.SoLuong <= 0 ? 1 : it.SoLuong;

                    // Check tồn
                    var ton = sp.So_Luong;
                    if (sl > ton)
                        overList.Add($"\"{sp.Ten_San_Pham}\": đặt {sl} > tồn {ton}");

                    // Giá sau KM
                    var giaSauKm = CalcPriceAfterPromo(sp, utcNow);

                    // Size + Topping
                    var sizeExtra = SIZE_META.TryGetValue(sizeId, out var meta) ? meta.extra : 0m;

                    decimal tienTopOne = 0m;
                    var topPayload = new List<object>();
                    foreach (var tid in topIds)
                    {
                        if (!toppingMap.TryGetValue(tid, out var top)) continue;
                        var g = top?.Gia ?? 0m;
                        tienTopOne += g;
                        topPayload.Add(new { ID_Topping = tid, Gia = g });
                    }

                    // Đơn giá 1 sản phẩm (sau KM) + size + topping
                    var donGiaOne = Math.Max(0m, giaSauKm) + Math.Max(0m, sizeExtra) + Math.Max(0m, tienTopOne);
                    var lineTotal = donGiaOne * sl;
                    tamTinh += lineTotal;

                    // Mã chi tiết duy nhất
                    var maCt = NewChiTietCode(lineNo);

                    // Build chi tiết cho BE — KHỚP EXACT tên trường của BE
                    chiTietList.Add(new
                    {
                        ID_HoaDon_ChiTiet = 0,
                        ID_San_Pham = it.SanPhamId,
                        ID_Size = sizeId,
                        ID_SanPham_DoNgot = doNgotId,
                        ID_LuongDa = luongDaId,

                        Ma_HoaDon_ChiTiet = maCt,

                        Gia_Them_Size = sizeExtra,
                        Gia_San_Pham = giaSauKm,    // GIÁ SAU KM (chưa gồm size/topping)
                        So_Luong = sl,
                        Tong_Tien = lineTotal,      // tổng dòng đã gồm size+topping
                        Ghi_Chu = "",
                        HoaDonChiTietToppings = topPayload
                    });
                }

                if (overList.Count > 0)
                    return Json(new { success = false, message = "Vượt tồn kho:\n" + string.Join("\n", overList) });

                // clamp giảm giá ≤ 50%
                var giam = Math.Max(0m, Math.Min(req.TienGiam, tamTinh * 0.5m));
                var thanhToan = Math.Max(0m, tamTinh - giam);

                // Loại HĐ & Địa chỉ
                var loai = (req.LoaiHoaDon ?? "TaiQuay").Trim();
                var allow = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "TaiQuay", "Online", "GiaoHang" };
                if (!allow.Contains(loai)) loai = "TaiQuay";

                string diaChiForBE = loai.Equals("TaiQuay", StringComparison.OrdinalIgnoreCase)
                    ? "Tại quầy"
                    : (req.DiaChiTuNhap ?? "");

                // Trạng thái
                string trangThai = loai.Equals("TaiQuay", StringComparison.OrdinalIgnoreCase) ? "Hoan_Thanh" : "Chua_Xac_Nhan";

                // Mã HĐ
                var now = DateTime.Now;
                var ma = string.IsNullOrWhiteSpace(req.MaHoaDon)
                    ? ("POS" + now.ToString("yyMMddHHmmss") + "-" + Guid.NewGuid().ToString("N")[..4])
                    : req.MaHoaDon;

                int ptttId = req.HinhThucThanhToanId ?? 1;

                var payload = new
                {
                    ID_Hoa_Don = 0,
                    ID_Khach_Hang = DEFAULT_KHACH_LE_ID,
                    ID_Nhan_Vien = DEFAULT_NHAN_VIEN_ID,
                    ID_Hinh_Thuc_Thanh_Toan = ptttId,
                    ID_Dia_Chi = (int?)null,
                    ID_Phi_Ship = (int?)null,

                    Dia_Chi_Tu_Nhap = diaChiForBE,
                    Ngay_Tao = now,
                    Tong_Tien = thanhToan,   // tổng cuối
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
                            continue; // thử endpoint khác
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

                // Lấy id/code nếu có
                string? returnedCode = null;
                int? returnedId = null;
                try
                {
                    var beObj = JsonDocument.Parse(respText).RootElement;
                    if (beObj.TryGetProperty("ma_Hoa_Don", out var codeProp) && codeProp.ValueKind == JsonValueKind.String)
                        returnedCode = codeProp.GetString();
                    if (beObj.TryGetProperty("ma_HoaDon", out var codeProp2) && codeProp2.ValueKind == JsonValueKind.String)
                        returnedCode = codeProp2.GetString() ?? returnedCode;
                    if (beObj.TryGetProperty("id_Hoa_Don", out var idProp) && idProp.TryGetInt32(out var idVal))
                        returnedId = idVal;
                    if (returnedCode == null && beObj.TryGetProperty("ma_Hoa_Don", out var any) && any.ValueKind == JsonValueKind.Number)
                        returnedCode = any.GetRawText();
                }
                catch { /* ignore */ }

                // ===== Trừ tồn kho — tắt mặc định để tránh mất dữ liệu do BE =====
                if (CALL_ADJUST_STOCK_AFTER_CREATE)
                {
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
                                message = "Tạo hoá đơn thành công, nhưng trừ tồn kho thất bại (khuyến nghị gộp transaction ở BE).",
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
                            message = "Tạo hoá đơn thành công, nhưng lỗi khi trừ tồn kho (khuyến nghị gộp transaction ở BE): " + exAdj.Message,
                            id = returnedId,
                            code = returnedCode
                        });
                    }
                }

                // ==== Giảm So_Luong voucher (-1) nếu có áp dụng ====
                if (giam > 0)
                {
                    try
                    {
                        HttpResponseMessage? consumeResp = null;

                        if (req.VoucherId.HasValue)
                        {
                            var urlById = $"/api/Voucher/consume/{req.VoucherId.Value}?qty=1";
                            consumeResp = await http.PostAsync(urlById, null);
                        }

                        if ((consumeResp == null || !consumeResp.IsSuccessStatusCode) &&
                            !string.IsNullOrWhiteSpace(req.VoucherCode))
                        {
                            var urlByCode = $"/api/Voucher/consume-by-code/{Uri.EscapeDataString(req.VoucherCode)}?qty=1";
                            consumeResp = await http.PostAsync(urlByCode, null);
                        }

                        if (consumeResp == null || !consumeResp.IsSuccessStatusCode)
                        {
                            var tx = consumeResp != null ? await consumeResp.Content.ReadAsStringAsync() : "no-response";
                            return Json(new
                            {
                                success = true,
                                message = "Tạo hoá đơn OK, nhưng trừ So_Luong voucher thất bại.",
                                id = returnedId,
                                code = returnedCode,
                                voucher_consume_status = consumeResp != null ? (int)consumeResp.StatusCode : 0,
                                voucher_consume_body = tx
                            });
                        }
                    }
                    catch (Exception exV)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Tạo hoá đơn OK, nhưng lỗi khi gọi consume voucher.",
                            id = returnedId,
                            code = returnedCode,
                            voucher_consume_error = exV.Message
                        });
                    }
                }

                return Json(new
                {
                    success = true,
                    message = "Tạo hoá đơn thành công.",
                    id = returnedId,
                    code = returnedCode
                });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // ===== (Optional) Endpoint proxy trừ tồn =====
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> TruTonKhoSauThanhToan([FromBody] List<AdjustStockRow> rows)
        {
            try
            {
                if (rows == null || rows.Count == 0)
                    return Json(new { success = false, message = "Thiếu dữ liệu trừ tồn." });

                using var http = new HttpClient { BaseAddress = new Uri(BeBaseUrl), Timeout = TimeSpan.FromSeconds(15) };
                var resp = await http.PostAsJsonAsync(AdjustStockEndpoint,
                    rows.Select(r => new { ID_San_Pham = r.ID_San_Pham, SoLuongTru = r.SoLuongTru }));
                var tx = await resp.Content.ReadAsStringAsync();
                if (!resp.IsSuccessStatusCode) return Json(new { success = false, message = $"BE {(int)resp.StatusCode}: {tx}" });
                return Json(new { success = true });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // ==================== DTOs (khớp payload từ view) =====================
        public class TaoHoaDonTaiQuayRequest
        {
            public string? MaHoaDon { get; set; }
            public int? HinhThucThanhToanId { get; set; }
            public string? LoaiHoaDon { get; set; }        // "TaiQuay" | "Online" | "GiaoHang"
            public string? KhachHang_SDT { get; set; }
            public string? GhiChu { get; set; }

            public int? VoucherId { get; set; }            // Ưu tiên theo ID
            public string? VoucherCode { get; set; }       // Fallback theo mã

            public decimal TongTien { get; set; }          // client gửi – server tự tính lại
            public decimal TienGiam { get; set; }          // clamp ≤ 50%
            public string? DiaChiTuNhap { get; set; }      // UI: chỉ dùng khi GiaoHang
            public List<TaoHoaDonTaiQuayItem> Items { get; set; } = new();
        }

        public class TaoHoaDonTaiQuayItem
        {
            public int SanPhamId { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }            // client gửi – server KHÔNG tin
            public int? DoNgotId { get; set; }
            public int? LuongDaId { get; set; }
            public List<int>? ToppingIds { get; set; }
            public int? SizeId { get; set; }               // 1/2/3
        }

        public class AdjustStockRow
        {
            public int ID_San_Pham { get; set; }
            public int SoLuongTru { get; set; }
        }

        // ==================== LẤY OPTION HỢP LỆ TỪ BE =====================
        private class ProductOptionsDTO
        {
            public List<int> SizeIds { get; set; } = new() { 1, 2, 3 };
            public List<int> DoNgotIds { get; set; } = new();
            public List<int> LuongDaIds { get; set; } = new();
            public List<int> ToppingIds { get; set; } = new();
        }

        private async Task<ProductOptionsDTO> GetAllowedOptionsFromSanPhamApi(int productId)
        {
            var dto = new ProductOptionsDTO();
            try
            {
                using var http = new HttpClient { BaseAddress = new Uri(BeBaseUrl), Timeout = TimeSpan.FromSeconds(10) };
                var res = await http.GetAsync($"/api/SanPham/{productId}");
                if (!res.IsSuccessStatusCode) return dto;

                var json = await res.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // sizes
                if (root.TryGetProperty("sizes", out var sizes) && sizes.ValueKind == JsonValueKind.Array)
                {
                    var lst = new List<int>();
                    foreach (var el in sizes.EnumerateArray())
                    {
                        var active = el.TryGetProperty("trang_Thai", out var st) ? st.GetBoolean() :
                                     el.TryGetProperty("Trang_Thai", out var st2) ? st2.GetBoolean() : true;
                        if (!active) continue;
                        if (el.TryGetProperty("iD_Size", out var id) && id.TryGetInt32(out var v)) lst.Add(v);
                    }
                    if (lst.Count > 0) dto.SizeIds = lst.Distinct().ToList();
                }

                // doNgots
                if (root.TryGetProperty("doNgots", out var dng) && dng.ValueKind == JsonValueKind.Array)
                {
                    foreach (var el in dng.EnumerateArray())
                    {
                        var active = el.TryGetProperty("trang_Thai", out var st) ? st.GetBoolean() :
                                     el.TryGetProperty("Trang_Thai", out var st2) ? st2.GetBoolean() : true;
                        if (!active) continue;
                        if (el.TryGetProperty("iD_DoNgot", out var id) && id.TryGetInt32(out var v)) dto.DoNgotIds.Add(v);
                    }
                    dto.DoNgotIds = dto.DoNgotIds.Distinct().ToList();
                }

                // luongDas
                if (root.TryGetProperty("luongDas", out var lda) && lda.ValueKind == JsonValueKind.Array)
                {
                    foreach (var el in lda.EnumerateArray())
                    {
                        var active = el.TryGetProperty("trang_Thai", out var st) ? st.GetBoolean() :
                                     el.TryGetProperty("Trang_Thai", out var st2) ? st2.GetBoolean() : true;
                        if (!active) continue;
                        if (el.TryGetProperty("iD_LuongDa", out var id) && id.TryGetInt32(out var v)) dto.LuongDaIds.Add(v);
                    }
                    dto.LuongDaIds = dto.LuongDaIds.Distinct().ToList();
                }

                // toppings
                if (root.TryGetProperty("toppings", out var tops) && tops.ValueKind == JsonValueKind.Array)
                {
                    foreach (var el in tops.EnumerateArray())
                    {
                        var active = el.TryGetProperty("trang_Thai", out var st) ? st.GetBoolean() :
                                     el.TryGetProperty("Trang_Thai", out var st2) ? st2.GetBoolean() : true;
                        if (!active) continue;

                        // phải còn hàng
                        if (el.TryGetProperty("so_Luong", out var sl) && sl.ValueKind == JsonValueKind.Number && sl.GetInt32() <= 0)
                            continue;

                        if (el.TryGetProperty("iD_Topping", out var id) && id.TryGetInt32(out var v)) dto.ToppingIds.Add(v);
                    }
                    dto.ToppingIds = dto.ToppingIds.Distinct().ToList();
                }
            }
            catch { /* dùng default */ }

            return dto;
        }
    }
}
