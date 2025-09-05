﻿using System;
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

        // Thử lần lượt các endpoint tạo hóa đơn (tuỳ BE)
        private static readonly string[] HoaDonEndpoints =
        {
            "/api/HoaDon",            // POST
            "/api/HoaDon/create",
            "/api/hoa-don",
            "/api/hoa-don/create"
        };

        // Các ID PHẢI tồn tại trong DB (đổi cho đúng hệ thống của bạn)
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
            ViewBag.Products = await _productService.GetAllProductsAsync();
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync();
            ViewBag.Toppings = await _productService.GetToppingsAsync();
            return View();
        }

        // ===== API: SẢN PHẨM (lọc trên client) =====
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetSanPham(string tuKhoa = "", int page = 1, int pageSize = 12)
        {
            try
            {
                var all = (await _productService.GetAllProductsAsync()) ?? new List<SanPham>();

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
                        ID_San_Pham = sp.ID_San_Pham,
                        Ten_San_Pham = sp.Ten_San_Pham ?? "",
                        Gia = sp.Gia,
                        Hinh_Anh = sp.Hinh_Anh ?? ""
                    })
                    .ToList();

                return Json(new { success = true, data = items, total, page, pageSize });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // ===== API: OPTIONS (độ ngọt / đá / topping) =====
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetOptions()
        {
            try
            {
                var doNgots = await _productService.GetDoNgotsAsync();
                var luongDas = await _productService.GetLuongDasAsync();
                var toppings = await _productService.GetToppingsAsync();

                return Json(new
                {
                    success = true,
                    doNgots = (doNgots ?? new List<DoNgot>()).Select(d => new { d.ID_DoNgot, d.Muc_Do }),
                    luongDas = (luongDas ?? new List<LuongDa>()).Select(l => new
                    {
                        l.ID_LuongDa,
                        Ten_LuongDa = l.Ten_LuongDa,
                        Muc_Da = l.Ten_LuongDa
                    }),
                    toppings = (toppings ?? new List<Topping>()).Select(t => new
                    {
                        t.ID_Topping,
                        Ten = t.Ten,
                        Gia = t.Gia,
                        t.Hinh_Anh
                    })
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

            if (ma.StartsWith("SALE") && ma.Length >= 6 && decimal.TryParse(ma.Substring(4), out var pct) && pct <= 100)
                giam = tongTien * (pct / 100m);
            else if (ma == "SALE100")
                giam = 100_000m;
            else
                return Json(new { success = false, message = "Voucher không hợp lệ." });

            var max = tongTien * 0.5m;
            if (giam > max) giam = Math.Floor(max);

            return Json(new { success = true, giam, thanhToan = Math.Max(0, tongTien - giam), voucher = new { ID_Voucher = 0, Ma_Voucher = ma } });
        }

        // ===== API: TẠO HÓA ĐƠN =====
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

                var now = DateTime.Now;
                var ma = string.IsNullOrWhiteSpace(req.MaHoaDon) ? ("POS" + now.ToString("yyMMddHHmmss")) : req.MaHoaDon;

                // 1/2/3 đã có sẵn trong DB
                int ptttId = req.HinhThucThanhToanId ?? 1;

                // Loại hoá đơn: lấy từ FE; nếu FE không gửi thì để default "TaiQuay"
                var loai = (req.LoaiHoaDon ?? "TaiQuay").Trim();
                // Nếu muốn whitelist:
                var allow = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "TaiQuay", "Online", "GiaoHang" };
                if (!allow.Contains(loai)) loai = "TaiQuay";

                // Chi tiết: dùng đúng key theo model BE
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

                    Dia_Chi_Tu_Nhap = "",
                    Ngay_Tao = now,
                    Tong_Tien = Math.Max(0, req.TongTien - req.TienGiam),
                    Phi_Ship = 0m,
                    Trang_Thai = "Chua_Xac_Nhan",
                    Ghi_Chu = req.GhiChu ?? "",
                    Ma_Hoa_Don = ma,
                    Loai_Hoa_Don = loai,

                    HoaDonChiTiets = chiTiets,
                    HoaDonVouchers = (req.VoucherId.HasValue && req.TienGiam > 0)
                        ? new[] { new { ID_HoaDonVoucher = 0, ID_Voucher = req.VoucherId.Value, Gia_Tri_Giam = req.TienGiam } }
                        : Array.Empty<object>(),
                    LichSuHoaDons = Array.Empty<object>()
                };

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

                if (resp == null) return Json(new { success = false, message = "Không kết nối được BE." });
                if (resp.IsSuccessStatusCode)
                    return Json(new { success = true, message = "Tạo hoá đơn thành công.", be = new { status = (int)resp.StatusCode, body = bodyText } });

                return Json(new { success = false, message = $"BE {(int)resp.StatusCode}: {bodyText}" });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }
    }

    // ===== DTO FE =====
    public class TaoHoaDonTaiQuayRequest
    {
        public string? MaHoaDon { get; set; }
        public int? HinhThucThanhToanId { get; set; }  // 1/2/3
        public string? LoaiHoaDon { get; set; }        // <— thêm mới: "TaiQuay" | "Online" | "GiaoHang"
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
