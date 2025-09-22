using BE.models;
using FE.Filters;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FE.Controllers
{
    [RoleAuthorize(2)] // Trang cho phép cả vai trò 2,3
    // Phương thức này đươc để trong thư mục Filters nhé ae
    public class ThongKeController : Controller
    {
        private readonly IHoaDonService _hoaDonService;

        public ThongKeController(IHoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }

        // Trang chính thống kê
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

        // Giữ nguyên nếu bạn cần view phụ theo trạng thái
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

        // Trang chi tiết đơn
        public async Task<IActionResult> ChiTietHoaDon(int id)
        {
            var hoaDon = await _hoaDonService.GetByIdAsync(id);
            if (hoaDon == null) return NotFound();
            return View(hoaDon);
        }

        // API JSON nếu cần lấy lại dữ liệu bằng AJAX
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
