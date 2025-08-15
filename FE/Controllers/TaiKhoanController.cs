using BE.models;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FE.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly ITaiKhoanService _taiKhoanService;
        private readonly INhanVienService _nhanVienService;

        public TaiKhoanController(ITaiKhoanService taiKhoanService, INhanVienService nhanVienService)
        {
            _taiKhoanService = taiKhoanService;
            _nhanVienService = nhanVienService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var taiKhoans = await _taiKhoanService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                taiKhoans = taiKhoans
                    .Where(t =>
                        t.Ten_Nguoi_Dung.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(t.Email) && t.Email.Contains(search, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            ViewData["Search"] = search;

            return View(taiKhoans);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string? searchNhanVien)
        {
            var nhanViens = await _nhanVienService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchNhanVien))
            {
                nhanViens = nhanViens
                    .Where(nv =>
                        nv.Ho_Ten.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(nv.Email) && nv.Email.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            ViewBag.NhanViens = nhanViens;
            ViewBag.SearchNhanVien = searchNhanVien;
            return View(new TaiKhoan());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaiKhoan tk, string? searchNhanVien)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }

            try
            {
                tk.Trang_Thai = true;
                await _taiKhoanService.AddAsync(tk);
                return RedirectToAction("Index", "TaiKhoan");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi thêm: {ex.Message}";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string? searchNhanVien)
        {
            var taiKhoan = await _taiKhoanService.GetByIdAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            var nhanViens = await _nhanVienService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchNhanVien))
            {
                nhanViens = nhanViens
                    .Where(nv =>
                        nv.Ho_Ten.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(nv.Email) && nv.Email.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            ViewBag.NhanViens = nhanViens;
            ViewBag.SearchNhanVien = searchNhanVien;
            return View(taiKhoan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaiKhoan tk, string? searchNhanVien)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }

            try
            {
                await _taiKhoanService.UpdateAsync(id, tk);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi cập nhật: {ex.Message}";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _taiKhoanService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}