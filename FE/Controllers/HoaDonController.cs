using BE.models;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FE.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly IHoaDonService _hoaDonService;
        public HoaDonController(IHoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var hoaDons = await _hoaDonService.GetAllAsync();
            return View(hoaDons);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var hoaDon = await _hoaDonService.GetByIdAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            return View(hoaDon);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                await _hoaDonService.AddAsync(hoaDon);
                return RedirectToAction(nameof(Index));
            }
            return View(hoaDon);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var hoaDon = await _hoaDonService.GetByIdAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            return View(hoaDon);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HoaDon hoaDon)
        {
            if (id != hoaDon.ID_Hoa_Don)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _hoaDonService.UpdateAsync(id, hoaDon);
                return RedirectToAction(nameof(Index));
            }
            return View(hoaDon);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var hoaDon = await _hoaDonService.GetByIdAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            await _hoaDonService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
