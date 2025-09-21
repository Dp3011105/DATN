using BE.models;                 // HoaDon, HoaDonChiTiet ...
using FE.Filters;
using FE.Service.IService;       // IProductService
using Microsoft.AspNetCore.Mvc;
using Service.IService;          // IHoaDonService
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace FE.Controllers
{
    [RoleAuthorize(2, 3)] // Trang cho phép cả vai trò 2 và 3
    public class QuanLyDonHangController : Controller
    {
        private readonly IHoaDonService _hoaDonService;
        private readonly IProductService _productService;

        // ===== CONFIG FOCUS (đẩy đơn lên đầu trong 30 phút) =====
        private const string FocusCookieName = "focus_orders";
        private const int FocusTtlMinutes = 30;

        private static readonly JsonSerializerOptions JsonOpt = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        private sealed class FocusItem
        {
            public int Id { get; set; }
            public DateTime Ts { get; set; } // UTC
        }

        // ===== Trạng thái trong DB =====
        private static readonly string[] DbStatuses = new[]
        {
            "Chua_Xac_Nhan","Da_Xac_Nhan","Dang_Xu_Ly","Dang_Giao_Hang","Hoan_Thanh","Huy_Don"
        };

        // ===== Thứ tự ưu tiên nhóm trạng thái =====
        private static readonly Dictionary<string, int> StatusPriority =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Chua_Xac_Nhan"] = 0,
                ["Da_Xac_Nhan"] = 1,
                ["Dang_Xu_Ly"] = 2,
                ["Dang_Giao_Hang"] = 3,
                ["Hoan_Thanh"] = 4, // hai nhóm cuối chỉ sort theo thời gian
                ["Huy_Don"] = 5
            };

        // Luồng chuyển hợp lệ (cho các nút bước trạng thái; riêng Hủy cho phép theo quy tắc đặt ra)
        private static readonly Dictionary<string, string[]> AllowedTransitions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Chua_Xac_Nhan"] = new[] { "Da_Xac_Nhan", "Huy_Don" },
                ["Da_Xac_Nhan"] = new[] { "Dang_Xu_Ly", "Huy_Don" },
                ["Dang_Xu_Ly"] = new[] { "Dang_Giao_Hang", "Huy_Don" },
                ["Dang_Giao_Hang"] = new[] { "Hoan_Thanh" },
                ["Hoan_Thanh"] = Array.Empty<string>(),
                ["Huy_Don"] = Array.Empty<string>()
            };

        public QuanLyDonHangController(IHoaDonService hoaDonService, IProductService productService)
        {
            _hoaDonService = hoaDonService;
            _productService = productService;
        }

        // ===== Helpers: Focus cookie =====
        private Dictionary<int, DateTime> GetFocusMap()
        {
            try
            {
                if (!Request.Cookies.TryGetValue(FocusCookieName, out var raw) || string.IsNullOrWhiteSpace(raw))
                    return new();

                var arr = JsonSerializer.Deserialize<List<FocusItem>>(raw, JsonOpt) ?? new();
                var now = DateTime.UtcNow;
                var cutoff = now.AddMinutes(-FocusTtlMinutes);

                var filtered = arr
                    .Where(x => x != null && x.Id > 0 && x.Ts > cutoff && x.Ts <= now)
                    .GroupBy(x => x.Id)
                    .ToDictionary(g => g.Key, g => g.Max(z => z.Ts));

                // ghi lại để dọn rác
                SetFocusMap(filtered);
                return filtered;
            }
            catch
            {
                return new();
            }
        }

        private void SetFocusMap(Dictionary<int, DateTime> map)
        {
            try
            {
                var list = map.Select(kv => new FocusItem { Id = kv.Key, Ts = kv.Value }).ToList();
                var val = JsonSerializer.Serialize(list, JsonOpt);
                Response.Cookies.Append(FocusCookieName, val, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(FocusTtlMinutes + 5)
                });
            }
            catch { /* ignore */ }
        }

        private void AddToFocus(int id)
        {
            if (id <= 0) return;
            var map = GetFocusMap();
            map[id] = DateTime.UtcNow;
            SetFocusMap(map);
        }

        private void RemoveFromFocus(int id)
        {
            var map = GetFocusMap();
            if (map.Remove(id))
                SetFocusMap(map);
        }

        private static int GroupIndex(string? status)
        {
            if (status != null && StatusPriority.TryGetValue(status, out var idx)) return idx;
            return 999; // không map thì đẩy xuống cuối
        }

        // ============== LIST ==============
        [HttpGet]
        public async Task<IActionResult> Index(string tuKhoa = "", string trangThai = "TẤT CẢ")
        {
            var vm = new QuanLyDonHangViewModel
            {
                TuKhoa = tuKhoa?.Trim() ?? "",
                TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "TẤT CẢ" : trangThai.Trim(),
                TrangThaiList = new List<string> { "TẤT CẢ" }.Concat(DbStatuses).ToList()
            };

            try
            {
                // lấy 1 lần, xử lý in-memory để tránh enumerate nhiều
                var list = (await _hoaDonService.GetAllAsync())?.ToList() ?? new();

                if (!string.IsNullOrWhiteSpace(vm.TuKhoa))
                {
                    var kw = vm.TuKhoa;
                    list = list.Where(hd =>
                               (hd.Ma_Hoa_Don?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                            || (hd.KhachHang?.Ho_Ten?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                            || (hd.KhachHang?.So_Dien_Thoai?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                        ).ToList();
                }

                if (!vm.TrangThai.Equals("TẤT CẢ", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.Where(hd => string.Equals(hd.Trang_Thai, vm.TrangThai, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // đọc focus
                var focusMap = GetFocusMap();
                var focusIds = new HashSet<int>(focusMap.Keys);

                // tách danh sách
                var focused = list
                    .Where(h => focusIds.Contains(h.ID_Hoa_Don))
                    .OrderByDescending(h => focusMap[h.ID_Hoa_Don]) // đơn vừa tương tác lên trên
                    .ThenByDescending(h => h.Ngay_Tao)
                    .ToList();

                var remaining = list
                    .Where(h => !focusIds.Contains(h.ID_Hoa_Don))
                    .OrderBy(h => GroupIndex(h.Trang_Thai))
                    .ThenByDescending(h => h.Ngay_Tao)
                    .ToList();

                vm.FocusedHoaDon = focused;
                vm.DanhSachHoaDon = remaining;
            }
            catch
            {
                vm.FocusedHoaDon = new();
                vm.DanhSachHoaDon = new();
            }

            return View(vm);
        }

        // ============== DETAIL ==============
        [HttpGet]
        public async Task<IActionResult> ChiTiet(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null) return NotFound();

            // Mọi lần người dùng vào chi tiết → đẩy lên “khung thao tác nhanh”
            AddToFocus(id);

            // 🔥 NẠP DANH MỤC TOPPING LÀM FALLBACK
            var allToppings = await _productService.GetToppingsAsync() ?? new List<FE.Models.Topping>();

            static string GetTopName(object t, int id)
            {
                var tp = t.GetType();
                var pi = tp.GetProperty("Ten") ?? tp.GetProperty("Ten_Topping") ?? tp.GetProperty("Name");
                var val = pi?.GetValue(t) as string;
                return string.IsNullOrWhiteSpace(val) ? $"T#{id}" : val.Trim();
            }
            static decimal GetTopPrice(object t)
            {
                var tp = t.GetType();
                var pi = tp.GetProperty("Gia") ?? tp.GetProperty("Gia_Topping") ?? tp.GetProperty("Price");
                var v = pi?.GetValue(t);
                return v switch
                {
                    decimal d => d,
                    double d => (decimal)d,
                    float f => (decimal)f,
                    int i => i,
                    long l => l,
                    _ => 0m
                };
            }

            var toppingMap = allToppings
                .GroupBy(x => x.ID_Topping)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var last = g.Last();
                        return (Ten: GetTopName(last, last.ID_Topping), Gia: GetTopPrice(last));
                    });

            ViewBag.ToppingMap = toppingMap;

            var vm = new ChiTietHoaDonViewModel
            {
                HoaDon = hd,
                ChiTiets = hd.HoaDonChiTiets?.OrderBy(x => x.ID_HoaDon_ChiTiet).ToList() ?? new()
            };
            return View(vm);
        }

        // ============== JSON cho modal hủy ==============
        [HttpGet]
        public async Task<IActionResult> ChiTietJson(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null)
                return Json(new { ok = false, items = Array.Empty<object>(), msg = "Not found" });

            // Fallback: tải danh sách sản phẩm
            var allProducts = await _productService.GetAllProductsAsync() ?? new List<FE.Models.SanPham>();
            var prodMap = allProducts
                .GroupBy(p => GetIntProp(p, "ID_San_Pham", "Id", "ProductId"))
                .Where(g => g.Key > 0)
                .ToDictionary(g => g.Key, g => (object)g.Last());

            string BuildTenSpLocal(HoaDonChiTiet ct)
            {
                var tenSp = GetStringProp(ct.SanPham, "Ten_San_Pham", "Ten", "Name", "TenSP");
                if (string.IsNullOrWhiteSpace(tenSp))
                {
                    var pid = ct.ID_San_Pham;
                    if (pid > 0 && prodMap.TryGetValue(pid, out var pObj))
                    {
                        tenSp = GetStringProp(pObj, "Ten_San_Pham", "Ten", "Name", "TenSP");
                    }
                }
                if (string.IsNullOrWhiteSpace(tenSp)) tenSp = $"SP#{ct.ID_San_Pham}";

                var sizeName = GetStringProp(ct.Size, "SizeName", "Ten_Size", "Ten", "Name");
                if (!string.IsNullOrWhiteSpace(sizeName)) tenSp += $" - Size {sizeName}";

                var toppingNames = new List<string>();
                var listTp = ct.HoaDonChiTietToppings;
                if (listTp != null)
                {
                    foreach (var tpLine in listTp)
                    {
                        var tp = tpLine?.Topping;
                        var n = GetStringProp(tp, "Ten", "Ten_Topping", "Name");
                        if (!string.IsNullOrWhiteSpace(n)) toppingNames.Add(n!);
                    }
                }
                if (toppingNames.Count > 0) tenSp += $" (Topping: {string.Join(", ", toppingNames)})";

                return tenSp;
            }

            var items = (hd.HoaDonChiTiets ?? new List<HoaDonChiTiet>())
                .OrderBy(ct => ct.ID_HoaDon_ChiTiet)
                .Select(ct =>
                {
                    bool daLam = GetFlag(ct, "Da_Lam", "IsPrepared", "IsDone", "DaLam") ||
                                 string.Equals(GetStringProp(ct, "Trang_Thai_Chi_Tiet", "TrangThaiChiTiet"), "Da_Lam", StringComparison.OrdinalIgnoreCase);

                    return new
                    {
                        id = ct.ID_HoaDon_ChiTiet,
                        ten = BuildTenSpLocal(ct),
                        soLuong = ct.So_Luong,
                        daLam
                    };
                })
                .ToList();

            return Json(new { ok = true, items });
        }

        // ===== Helpers (reflection-safe) =====
        private static string? GetStringProp(object? obj, params string[] candidates)
        {
            if (obj == null || candidates == null || candidates.Length == 0) return null;
            var t = obj.GetType();
            foreach (var name in candidates)
            {
                var pi = t.GetProperty(name);
                if (pi == null) continue;
                var val = pi.GetValue(obj) as string;
                if (!string.IsNullOrWhiteSpace(val)) return val;
            }
            return null;
        }

        private static bool GetFlag(object? obj, params string[] candidates)
        {
            if (obj == null || candidates == null || candidates.Length == 0) return false;
            var t = obj.GetType();
            foreach (var name in candidates)
            {
                var pi = t.GetProperty(name);
                if (pi == null) continue;
                var val = pi.GetValue(obj);
                if (val is bool b) return b;
                if (val is int i) return i != 0;
            }
            return false;
        }

        private static int GetIntProp(object? obj, params string[] candidates)
        {
            if (obj == null || candidates == null || candidates.Length == 0) return 0;
            var t = obj.GetType();
            foreach (var name in candidates)
            {
                var pi = t.GetProperty(name);
                if (pi == null) continue;
                var v = pi.GetValue(obj);
                if (v is int i) return i;
                if (v is long l) return (int)l;
                if (v is short s) return s;
                if (v is string str && int.TryParse(str, out var parsed)) return parsed;
            }
            return 0;
        }

        // ============== STATE TRANSITIONS ==============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null) return NotFound();

            var curr = hd.Trang_Thai ?? "";
            var next = "Da_Xac_Nhan";

            if (!AllowedTransitions.TryGetValue(curr, out var allows) || !allows.Contains(next, StringComparer.OrdinalIgnoreCase))
            {
                TempData["msg"] = "Trạng thái hiện tại không cho phép xác nhận.";
                return RedirectToAction(nameof(Index));
            }

            var ok = await _hoaDonService.UpdateTrangThaiAsync(id, next, null);
            if (ok) AddToFocus(id); // đẩy lên khung
            TempData["msg"] = ok ? "Đã xác nhận đơn. Tiếp theo hãy 'Bắt đầu xử lý'." : "Cập nhật thất bại.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatDauXuLy(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null) return NotFound();

            var curr = hd.Trang_Thai ?? "";
            var next = "Dang_Xu_Ly";

            if (!AllowedTransitions.TryGetValue(curr, out var allows) || !allows.Contains(next, StringComparer.OrdinalIgnoreCase))
            {
                TempData["msg"] = "Chỉ có thể chuyển sang 'Đang xử lý' khi đơn đã xác nhận.";
                return RedirectToAction(nameof(Index));
            }

            var ok = await _hoaDonService.UpdateTrangThaiAsync(id, next, null);
            if (ok) AddToFocus(id);
            TempData["msg"] = ok ? "Đơn đã chuyển sang Đang xử lý." : "Cập nhật thất bại.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatDauGiaoHang(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null) return NotFound();

            var curr = hd.Trang_Thai ?? "";
            var next = "Dang_Giao_Hang";

            if (!AllowedTransitions.TryGetValue(curr, out var allows) || !allows.Contains(next, StringComparer.OrdinalIgnoreCase))
            {
                TempData["msg"] = "Chỉ có thể 'Bắt đầu giao' khi đơn đang ở trạng thái Đang xử lý.";
                return RedirectToAction(nameof(Index));
            }

            var ok = await _hoaDonService.UpdateTrangThaiAsync(id, next, null);
            if (ok) AddToFocus(id);
            TempData["msg"] = ok ? "Đơn đã chuyển sang Đang giao hàng." : "Cập nhật thất bại.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiaoHangThanhCong(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null) return NotFound();

            var curr = hd.Trang_Thai ?? "";
            var next = "Hoan_Thanh";

            if (!AllowedTransitions.TryGetValue(curr, out var allows) || !allows.Contains(next, StringComparer.OrdinalIgnoreCase))
            {
                TempData["msg"] = "Chỉ có thể hoàn thành khi đơn đang ở trạng thái Đang giao hàng.";
                return RedirectToAction(nameof(Index));
            }

            var ok = await _hoaDonService.UpdateTrangThaiAsync(id, next, null);
            if (ok) AddToFocus(id);
            TempData["msg"] = ok ? "Đã xác nhận giao hàng thành công." : "Cập nhật thất bại.";
            return RedirectToAction(nameof(Index));
        }

        // ============== HỦY + KHÔI PHỤC TỒN (theo quy tắc mới) ==============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Huy(int id, string lyDo, int[]? khoiPhucIds, int[]? khoiPhucQtys)
        {
            if (string.IsNullOrWhiteSpace(lyDo))
            {
                TempData["msg"] = "Vui lòng nhập lý do hủy.";
                return RedirectToAction(nameof(Index));
            }

            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null) return NotFound();

            var curr = hd.Trang_Thai?.Trim() ?? "";

            // Không cho hủy nếu đã hoàn thành hoặc đã hủy
            if (string.Equals(curr, "Hoan_Thanh", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(curr, "Huy_Don", StringComparison.OrdinalIgnoreCase))
            {
                TempData["msg"] = "Đơn đã hoàn tất hoặc đã huỷ. Không thể huỷ.";
                return RedirectToAction(nameof(Index));
            }

            var chiTiets = hd.HoaDonChiTiets ?? new List<HoaDonChiTiet>();
            var mapCt = chiTiets.ToDictionary(x => x.ID_HoaDon_ChiTiet, x => x);

            bool restockOk = true;
            int totalRestockQty = 0;

            // ========== QUY TẮC ==========
            // 1) Chua_Xac_Nhan => KHÔI PHỤC TOÀN BỘ TỒN tự động
            if (string.Equals(curr, "Chua_Xac_Nhan", StringComparison.OrdinalIgnoreCase))
            {
                var restockAll = chiTiets
                    .GroupBy(ct => ct.ID_San_Pham)
                    .Select(g => (productId: g.Key, quantity: g.Sum(ct => Math.Max(0, ct.So_Luong))))
                    .Where(x => x.productId > 0 && x.quantity > 0)
                    .ToList();

                if (restockAll.Count > 0)
                {
                    restockOk = await _productService.RestockBatchAsync(restockAll);
                    totalRestockQty = restockAll.Sum(x => x.quantity);
                }

                var updateOk = await _hoaDonService.UpdateTrangThaiAsync(id, "Huy_Don", lyDo.Trim());
                if (updateOk) AddToFocus(id);

                TempData["msg"] = (updateOk && restockOk)
                    ? $"Đã hủy đơn và khôi phục toàn bộ tồn ({totalRestockQty} sp)."
                    : (updateOk ? "Đã hủy đơn nhưng KHÔNG khôi phục tồn (lỗi cập nhật sản phẩm)."
                                : "Hủy đơn thất bại.");

                return RedirectToAction(nameof(Index));
            }

            // 2) Dang_Giao_Hang => HỦY KHÔNG KHÔI PHỤC TỒN
            if (string.Equals(curr, "Dang_Giao_Hang", StringComparison.OrdinalIgnoreCase))
            {
                var updateOk = await _hoaDonService.UpdateTrangThaiAsync(id, "Huy_Don", lyDo.Trim());
                if (updateOk) AddToFocus(id);

                TempData["msg"] = updateOk
                    ? "Đã hủy đơn trong trạng thái Đang giao hàng (KHÔNG khôi phục tồn)."
                    : "Hủy đơn thất bại.";

                return RedirectToAction(nameof(Index));
            }

            // 3) Da_Xac_Nhan hoặc Dang_Xu_Ly => CHO PHÉP CHỌN SỐ LƯỢNG ĐỂ KHÔI PHỤC
            if (string.Equals(curr, "Da_Xac_Nhan", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(curr, "Dang_Xu_Ly", StringComparison.OrdinalIgnoreCase))
            {
                var selections = new List<(int chiTietId, int soLuong)>();
                if (khoiPhucIds != null && khoiPhucQtys != null && khoiPhucIds.Length == khoiPhucQtys.Length)
                {
                    for (int i = 0; i < khoiPhucIds.Length; i++)
                    {
                        var cid = khoiPhucIds[i];
                        var q = Math.Max(0, khoiPhucQtys[i]);
                        if (cid <= 0 || q <= 0) continue;

                        if (!mapCt.TryGetValue(cid, out var ct)) continue;

                        var max = Math.Max(0, ct.So_Luong);
                        if (q > max) q = max;
                        if (q > 0) selections.Add((cid, q));
                    }
                }

                var restockByProduct = selections
                    .Select(sel =>
                    {
                        var ct = mapCt[sel.chiTietId];
                        return new { productId = ct.ID_San_Pham, qty = sel.soLuong };
                    })
                    .Where(x => x.productId > 0 && x.qty > 0)
                    .GroupBy(x => x.productId)
                    .Select(g => (productId: g.Key, quantity: g.Sum(z => z.qty)))
                    .ToList();

                if (restockByProduct.Count > 0)
                {
                    restockOk = await _productService.RestockBatchAsync(restockByProduct);
                    totalRestockQty = restockByProduct.Sum(x => x.quantity);
                }

                var updateStatusOk = await _hoaDonService.UpdateTrangThaiAsync(id, "Huy_Don", lyDo.Trim());
                if (updateStatusOk) AddToFocus(id);

                TempData["msg"] = (updateStatusOk && restockOk)
                    ? $"Đã huỷ đơn và khôi phục tồn theo lựa chọn ({totalRestockQty} sp)."
                    : (updateStatusOk ? "Đã huỷ đơn nhưng KHÔNG khôi phục tồn (lỗi cập nhật sản phẩm)."
                                      : "Hủy đơn thất bại.");

                return RedirectToAction(nameof(Index));
            }

            // Các trạng thái khác (phòng thủ): hủy nhưng không khôi phục
            {
                var updateOk = await _hoaDonService.UpdateTrangThaiAsync(id, "Huy_Don", lyDo.Trim());
                if (updateOk) AddToFocus(id);

                TempData["msg"] = updateOk
                    ? "Đã hủy đơn."
                    : "Hủy đơn thất bại.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ============== Gỡ đơn khỏi khung thao tác nhanh ==============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BoFocus(int id)
        {
            RemoveFromFocus(id);
            return RedirectToAction(nameof(Index));
        }
    }

    // ============== ViewModels ==============
    public class QuanLyDonHangViewModel
    {
        public List<HoaDon> FocusedHoaDon { get; set; } = new(); // khung thao tác nhanh
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
