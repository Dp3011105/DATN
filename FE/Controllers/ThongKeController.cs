using Microsoft.AspNetCore.Mvc;
using BE.models;
using Service.IService;

namespace FE.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly IHoaDonService _hoaDonService;

        public ThongKeController(IHoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var hoaDons = await _hoaDonService.GetAllAsync() ?? new List<HoaDon>();
                return View(hoaDons);
            }
            catch
            {
                return View(new List<HoaDon>());
            }
        }

        public async Task<IActionResult> ThongKeTheoTrangThai()
        {
            try
            {
                var hoaDons = await _hoaDonService.GetAllAsync() ?? new List<HoaDon>();
                return View(hoaDons);
            }
            catch
            {
                return View(new List<HoaDon>());
            }
        }

        public async Task<IActionResult> ChiTietHoaDon(int id)
        {
            var hoaDon = await _hoaDonService.GetByIdAsync(id);
            if (hoaDon == null) return NotFound();
            return View(hoaDon);
        }

        [HttpGet]
        public async Task<IActionResult> GetThongKeData()
        {
            try
            {
                var hoaDons = await _hoaDonService.GetAllAsync();
                return Json(new { success = true, data = hoaDons ?? new List<HoaDon>() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
