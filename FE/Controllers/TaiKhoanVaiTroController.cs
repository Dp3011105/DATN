using BE.DTOs;
using BE.models;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FE.Controllers
{
    public class TaiKhoanVaiTroController : Controller
    {
        private readonly ITaiKhoanVaiTroService _taiKhoanVaiTroService;

        public TaiKhoanVaiTroController(ITaiKhoanVaiTroService taiKhoanVaiTroService)
        {
            _taiKhoanVaiTroService = taiKhoanVaiTroService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var taiKhoanVaiTros = await _taiKhoanVaiTroService.GetAllTaiKhoanVaiTroAsync();
            return View(taiKhoanVaiTros);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var taiKhoans = await _taiKhoanVaiTroService.GetAllTaiKhoanNhanVienAsync();
            var vaiTros = await _taiKhoanVaiTroService.GetAllVaiTroAsync();
            ViewBag.TaiKhoans = taiKhoans;
            ViewBag.VaiTros = vaiTros;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaiKhoanVaiTro taiKhoanVaiTro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _taiKhoanVaiTroService.CreateTaiKhoanVaiTroAsync(taiKhoanVaiTro);
                    return RedirectToAction("Index","TaiKhoanVaiTro");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm dữ liệu: " + ex.Message);
                }
            }
            var taiKhoans = await _taiKhoanVaiTroService.GetAllTaiKhoanNhanVienAsync();
            var vaiTros = await _taiKhoanVaiTroService.GetAllVaiTroAsync();
            ViewBag.TaiKhoans = taiKhoans;
            ViewBag.VaiTros = vaiTros;
            return View(taiKhoanVaiTro);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int idTaiKhoan, int idVaiTro)
        {
            var taiKhoanVaiTro = await _taiKhoanVaiTroService.GetTaiKhoanVaiTroByIdAsync(idTaiKhoan, idVaiTro);
            if (taiKhoanVaiTro == null)
            {
                return NotFound();
            }
            var model = new TaiKhoanVaiTro
            {
                ID_Tai_Khoan = taiKhoanVaiTro.ID_Tai_Khoan,
                ID_Vai_Tro = taiKhoanVaiTro.ID_Vai_Tro
            };
            var taiKhoans = await _taiKhoanVaiTroService.GetAllTaiKhoanNhanVienAsync();
            var vaiTros = await _taiKhoanVaiTroService.GetAllVaiTroAsync();
            ViewBag.TaiKhoans = taiKhoans;
            ViewBag.VaiTros = vaiTros;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idTaiKhoan, int idVaiTro, TaiKhoanVaiTro taiKhoanVaiTro)
        {
            if (idTaiKhoan != taiKhoanVaiTro.ID_Tai_Khoan || idVaiTro != taiKhoanVaiTro.ID_Vai_Tro)
            {
                return BadRequest("ID không khớp.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _taiKhoanVaiTroService.UpdateTaiKhoanVaiTroAsync(taiKhoanVaiTro);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật dữ liệu: " + ex.Message);
                }
            }
            var taiKhoans = await _taiKhoanVaiTroService.GetAllTaiKhoanNhanVienAsync();
            var vaiTros = await _taiKhoanVaiTroService.GetAllVaiTroAsync();
            ViewBag.TaiKhoans = taiKhoans;
            ViewBag.VaiTros = vaiTros;
            return View(taiKhoanVaiTro);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idTaiKhoan, int idVaiTro)
        {
            var taiKhoanVaiTro = await _taiKhoanVaiTroService.GetTaiKhoanVaiTroByIdAsync(idTaiKhoan, idVaiTro);
            if (taiKhoanVaiTro == null)
            {
                return NotFound();
            }
            try
            {
                await _taiKhoanVaiTroService.DeleteTaiKhoanVaiTroAsync(idTaiKhoan, idVaiTro);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi xóa dữ liệu: " + ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}