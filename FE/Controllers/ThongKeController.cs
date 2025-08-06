using Microsoft.AspNetCore.Mvc;
using BE.models;

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
                var hoaDons = await _hoaDonService.GetAllAsync();

                // Kiểm tra null để tránh lỗi
                if (hoaDons == null)
                {
                    hoaDons = new List<HoaDonDTO>();
                }

                return View(hoaDons);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                // _logger.LogError(ex, "Error occurred while getting hoa don data");

                // Trả về view với danh sách rỗng nếu có lỗi
                return View(new List<HoaDonDTO>());
            }
        }

        // Thêm các action khác cho thống kê
        public async Task<IActionResult> ThongKeTheoTrangThai()
        {
            try
            {
                var hoaDons = await _hoaDonService.GetAllAsync();

                if (hoaDons == null)
                {
                    return View(new List<HoaDonDTO>());
                }

                // Có thể thêm logic thống kê theo trạng thái ở đây
                return View(hoaDons);
            }
            catch (Exception ex)
            {
                return View(new List<HoaDonDTO>());
            }
        }

        public async Task<IActionResult> ChiTietHoaDon(int id)
        {
            try
            {
                var hoaDon = await _hoaDonService.GetByIdAsync(id);

                if (hoaDon == null)
                {
                    return NotFound();
                }

                return View(hoaDon);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // API endpoint để lấy dữ liệu cho biểu đồ (nếu cần)
        [HttpGet]
        public async Task<IActionResult> GetThongKeData()
        {
            try
            {
                var hoaDons = await _hoaDonService.GetAllAsync();

                if (hoaDons == null)
                {
                    return Json(new { success = false, data = new List<HoaDonDTO>() });
                }

                return Json(new { success = true, data = hoaDons });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}