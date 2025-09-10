using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace FE.Controllers
{
    public class QuanLyVaiTroNhanVienController : Controller
    {
        private readonly IQuanLyVaiTroService _service;

        public QuanLyVaiTroNhanVienController(IQuanLyVaiTroService service)
        {
            _service = service;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var viewModel = new QuanLyVaiTroViewModel
        //    {
        //        NhanVienKhongVaiTro = await _service.GetNhanVienKhongVaiTro(),
        //        NhanVienCoVaiTro = await _service.GetNhanVienCoVaiTro(),
        //        VaiTros = await _service.GetAllVaiTro()
        //    };

        //    return View(viewModel);
        //}
          public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GanVaiTro(int idTaiKhoan, int[] danhSachVaiTro)
        {
            try
            {
                var request = new GanVaiTroRequest
                {
                    ID_Tai_Khoan = idTaiKhoan,
                    Danh_Sach_ID_Vai_Tro = danhSachVaiTro.ToList()
                };
                ViewBag.RequestLog = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
                await _service.GanVaiTro(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Lỗi khi gán vai trò: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatVaiTro(int idTaiKhoan, int[] danhSachVaiTro)
        {
            try
            {
                var request = new GanVaiTroRequest
                {
                    ID_Tai_Khoan = idTaiKhoan,
                    Danh_Sach_ID_Vai_Tro = danhSachVaiTro.ToList()
                };
                ViewBag.RequestLog = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
                await _service.CapNhatVaiTro(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Lỗi khi cập nhật vai trò: {ex.Message}";
                return View();
            }
        }
    }
}
