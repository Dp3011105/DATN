using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FE.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            if (ModelState.IsValid)
            {
                var success = await _authService.RegisterAsync(model);
                if (success)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Đăng ký thất bại.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _authService.LoginAsync(model);
                    if (response.VaiTros != null && response.VaiTros.Contains(1))
                    {
                        // Lưu dữ liệu vào cookie
                        var userDataJson = JsonSerializer.Serialize(response);
                        Response.Cookies.Append("UserData", userDataJson, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, // Sử dụng true nếu dùng HTTPS
                            Expires = DateTimeOffset.UtcNow.AddHours(24) // Thời gian hết hạn, ví dụ 24 giờ
                        });

                        return RedirectToAction("Index", "HomeKhachHang");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Bạn không có quyền truy cập.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserData");
            return RedirectToAction("Index", "Home");
        }


    }
}
