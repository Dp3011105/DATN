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

        private static readonly string[] DbStatuses = new[]
        {
            "Chua_Xac_Nhan","Da_Xac_Nhan","Dang_Xu_Ly","Dang_Giao_Hang","Hoan_Thanh","Huy_Don"
        };

        public QuanLyDonHangController(IHoaDonService hoaDonService, IProductService productService)
        {
            _hoaDonService = hoaDonService;
            _productService = productService;
        }

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
                var list = (await _hoaDonService.GetAllAsync())?.ToList() ?? new();

                if (!string.IsNullOrWhiteSpace(vm.TuKhoa))
                {
                    var kw = vm.TuKhoa;
                    list = list.Where(hd =>
                           (hd.Ma_Hoa_Don?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                        || (hd.KhachHang?.Ho_Ten?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)
                    ).ToList();
                }

                if (!vm.TrangThai.Equals("TẤT CẢ", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.Where(hd => string.Equals(hd.Trang_Thai, vm.TrangThai, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                vm.DanhSachHoaDon = list
                    .OrderBy(h => string.Equals(h.Trang_Thai, "Chua_Xac_Nhan", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                    .ThenByDescending(h => h.Ngay_Tao)
                    .ToList();
            }
            catch { vm.DanhSachHoaDon = new(); }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ChiTiet(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null) return NotFound();
            var vm = new ChiTietHoaDonViewModel
            {
                HoaDon = hd,
                ChiTiets = hd.HoaDonChiTiets?.OrderBy(x => x.ID_HoaDon_ChiTiet).ToList() ?? new()
            };
            return View(vm);
        }

        // ================= JSON CHO MODAL HỦY (KHÔNG ĐỤNG MODEL, DÙNG REFLECTION) =================
        [HttpGet]
        public async Task<IActionResult> ChiTietJson(int id)
        {
            var hd = await _hoaDonService.GetByIdAsync(id);
            if (hd == null)
                return Json(new { ok = false, items = Array.Empty<object>(), msg = "Not found" });

            var items = (hd.HoaDonChiTiets ?? new List<HoaDonChiTiet>())
                .OrderBy(ct => ct.ID_HoaDon_ChiTiet)
                .Select(ct => new
                {
                    id = ct.ID_HoaDon_ChiTiet,
                    ten = BuildTenSp(ct),       // <<== dùng helper reflection
                    soLuong = ct.So_Luong,
                    daLam = false               // nếu có cờ ở chi tiết, set thật tại đây
                })
                .ToList();

            return Json(new { ok = true, items });
        }

        // ===== Helpers reflection an toàn compile-time =====
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

        private static string BuildTenSp(HoaDonChiTiet ct)
        {
            // SanPham: thử các tên khả dĩ
            var tenSp = GetStringProp(ct.SanPham, "Ten_San_Pham", "Ten", "Name") ?? "Sản phẩm";

            // Size
            var sizeName = GetStringProp(ct.Size, "SizeName", "Ten_Size", "Ten", "Name");

            // Toppings
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

            if (!string.IsNullOrWhiteSpace(sizeName)) tenSp += $" - Size {sizeName}";
            if (toppingNames.Count > 0) tenSp += $" (Topping: {string.Join(", ", toppingNames)})";
            return tenSp;
        }
        // ===========================================================================================

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

        // ====== HỦY + RESTOCK: NHẬN selections VÀ GỌI API MỚI ======
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Huy(int id, string lyDo, int[] khoiPhucIds, int[] khoiPhucQtys)
        {
            if (string.IsNullOrWhiteSpace(lyDo))
            {
                TempData["msg"] = "Vui lòng nhập lý do hủy.";
                return RedirectToAction(nameof(Index));
            }

            var selections = new List<(int chiTietId, int soLuong)>();
            if (khoiPhucIds != null && khoiPhucQtys != null && khoiPhucIds.Length == khoiPhucQtys.Length)
            {
                for (int i = 0; i < khoiPhucIds.Length; i++)
                {
                    var cid = khoiPhucIds[i];
                    var q = Math.Max(0, khoiPhucQtys[i]);
                    if (cid > 0 && q > 0) selections.Add((cid, q));
                }
            }

            var ok = await _hoaDonService.CancelWithRestockAsync(id, lyDo, selections);

            TempData["msg"] = ok
                ? "Đã hủy đơn và khôi phục tồn kho cho các cốc được chọn."
                : "Hủy đơn thất bại (không khôi phục tồn kho).";

            return RedirectToAction(nameof(Index));
        }
    }

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
