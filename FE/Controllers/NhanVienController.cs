using BE.models;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FE.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly INhanVienService _service;

        public NhanVienController(INhanVienService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "", bool? statusFilter = null, int page = 1, int pageSize = 10)
        {
            var result = await _service.GetAllAsync();
            var filteredResult = result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredResult = filteredResult.Where(nv => nv.Ho_Ten.Contains(searchTerm));
            }

            if (statusFilter.HasValue)
            {
                filteredResult = filteredResult.Where(nv => nv.Trang_Thai == statusFilter.Value);
            }

            var totalItems = filteredResult.Count();
            var paginatedResult = filteredResult
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.StatusFilter = statusFilter;

            return View(paginatedResult);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return PartialView("Details", result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhanVien entity, IFormFile anhNhanVien)
        {
            if (anhNhanVien != null && anhNhanVien.Length > 0)
            {
                var fileName = Path.GetFileName(anhNhanVien.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhNhanVien.CopyToAsync(stream);
                }

                entity.AnhNhanVien = "/uploads/" + fileName;
            }

            await _service.AddAsync(entity);
            return RedirectToAction("Index", "NhanVien");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            ViewBag.IsAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhanVien entity, IFormFile anhNhanVien)
        {
            if (anhNhanVien != null && anhNhanVien.Length > 0)
            {
                var fileName = Path.GetFileName(anhNhanVien.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhNhanVien.CopyToAsync(stream);
                }

                entity.AnhNhanVien = "/uploads/" + fileName;
            }
            entity.ID_Nhan_Vien = id;
            await _service.UpdateAsync(id, entity);
            return RedirectToAction("Index", "NhanVien");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}