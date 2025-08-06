using BE.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FE.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var customers = await _khachHangService.GetAllKhachHang();
            var totalItems = customers.Count();
            var paginatedCustomers = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(paginatedCustomers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _khachHangService.GetKhachHangById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View("Details", customer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KhachHangDTO entity)
        {
            if (ModelState.IsValid)
            {
                await _khachHangService.AddKhachHang(entity);
                return RedirectToAction("Index", "KhachHang");
            }
            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _khachHangService.GetKhachHangById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhachHangDTO entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _khachHangService.UpdateKhachHang(id, entity);
                    return RedirectToAction("Index","KhachHang");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _khachHangService.DeleteKhachHang(id);
                return RedirectToAction("Index", "KhachHang");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}