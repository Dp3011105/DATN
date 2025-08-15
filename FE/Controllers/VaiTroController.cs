using BE.models;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FE.Controllers
{
    public class VaiTroController : Controller
    {
        private readonly IVaiTroService _vaiTroService;

        public VaiTroController(IVaiTroService vaiTroService)
        {
            _vaiTroService = vaiTroService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vaiTros = await _vaiTroService.GetAllAsync();
            return View(vaiTros);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vaiTro = await _vaiTroService.GetByIdAsync(id);
            if (vaiTro == null)
            {
                return NotFound();
            }
            return View(vaiTro);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VaiTro vaiTro)
        {
            if (ModelState.IsValid)
            {
                await _vaiTroService.AddAsync(vaiTro);
                return RedirectToAction("Index", "VaiTro");
            }
            return View(vaiTro);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vaiTro = await _vaiTroService.GetByIdAsync(id);
            if (vaiTro == null)
            {
                return NotFound();
            }
            return View(vaiTro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VaiTro vaiTro)
        {
            if (id != vaiTro.ID_Vai_Tro)
            {
                return BadRequest("Role ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _vaiTroService.UpdateAsync(id, vaiTro);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
                return RedirectToAction("Index", "VaiTro");
            }
            return View(vaiTro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _vaiTroService.DeleteAsync(id);
                return RedirectToAction("Index", "VaiTro");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
