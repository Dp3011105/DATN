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

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var success = await _authService.RegisterAsync(model);
        //        if (success)
        //        {
        //            return RedirectToAction("Login");
        //        }
        //        ModelState.AddModelError("", "Đăng ký thất bại.");
        //    }
        //    return View(model);
        //}



        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var (success, message) = await _authService.RegisterAsync(model);
        //        if (success)
        //        {
        //            TempData["Success"] = message; // Lưu thông báo thành công vào TempData
        //            return RedirectToAction("Login");
        //        }
        //        else
        //        {
        //            TempData["Error"] = message; // Lưu thông báo lỗi vào TempData
        //            ModelState.AddModelError("", message);
        //        }
        //    }
        //    return View(model);
        //}



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                return View(model);
            }

          

            // Gọi dịch vụ đăng ký
            var (success, message) = await _authService.RegisterAsync(model);
            if (success)
            {
                TempData["Success"] = message;
                return RedirectToAction("Login");
            }
            else
            {
                TempData["Error"] = message;
                ModelState.AddModelError("", message);
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var response = await _authService.LoginAsync(model);
        //            if (response.VaiTros != null && response.VaiTros.Contains(1))
        //            {
        //                // Lưu dữ liệu vào cookie
        //                var userDataJson = JsonSerializer.Serialize(response);
        //                Response.Cookies.Append("UserData", userDataJson, new CookieOptions
        //                {
        //                    HttpOnly = false,
        //                    Secure = true, // Sử dụng true nếu dùng HTTPS
        //                    Expires = DateTimeOffset.UtcNow.AddHours(24) // Thời gian hết hạn, ví dụ 24 giờ
        //                });

        //                return RedirectToAction("Index", "HomeKhachHang");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Bạn không có quyền truy cập.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //        }
        //    }
        //    return View(model);
        //}




        //[HttpPost]
        //public async Task<IActionResult> Login(LoginModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var response = await _authService.LoginAsync(model);
        //            if (response.VaiTros != null)
        //            {
        //                // Lưu dữ liệu vào cookie
        //                var userDataJson = JsonSerializer.Serialize(response);
        //                Response.Cookies.Append("UserData", userDataJson, new CookieOptions
        //                {
        //                    HttpOnly = false,
        //                    Secure = true, // Sử dụng true nếu dùng HTTPS
        //                    Expires = DateTimeOffset.UtcNow.AddHours(24) // Thời gian hết hạn, ví dụ 24 giờ
        //                });

        //                // Kiểm tra vai trò và chuyển hướng
        //                if (response.VaiTros.Contains(2) ) // Admin 
        //                {
        //                    return RedirectToAction("Index", "ThongKe");
        //                }
        //                else if (response.VaiTros.Contains(1)) // Khách Hàng
        //                {
        //                    return RedirectToAction("Index", "HomeKhachHang");
        //                }
        //                else if (response.VaiTros.Contains(3)) // nhan vien
        //                {
        //                    return RedirectToAction("Index", "QuanLyDonHang");
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Bạn không có vai trò hợp lệ để truy cập.");
        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Bạn không có quyền truy cập.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //        }
        //    }
        //    return View(model);
        //}


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _authService.LoginAsync(model);
                    if (response.VaiTros != null)
                    {
                        // Lưu dữ liệu vào cookie
                        var userDataJson = JsonSerializer.Serialize(response);
                        Response.Cookies.Append("UserData", userDataJson, new CookieOptions
                        {
                            HttpOnly = false,
                            Secure = true,
                            Expires = DateTimeOffset.UtcNow.AddHours(24)
                        });

                        // Kiểm tra vai trò và chuyển hướng
                        if (response.VaiTros.Contains(2)) // Admin 
                        {
                            return RedirectToAction("Index", "ThongKe");
                        }
                        else if (response.VaiTros.Contains(1)) // Khách Hàng
                        {
                            return RedirectToAction("Index", "HomeKhachHang");
                        }
                        else if (response.VaiTros.Contains(3)) // Nhân viên
                        {
                            return RedirectToAction("Index", "QuanLyDonHang");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Bạn không có vai trò hợp lệ để truy cập.");
                        }
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




        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _authService.ForgotPasswordAsync(model.Email);
            if (success)
            {
                TempData["Success"] = message; // Ví dụ: "Mật khẩu mới đã được gửi qua email."
                return RedirectToAction("Login");
            }
            else
            {
                TempData["Error"] = message; // Ví dụ: "Email không tồn tại."
                ModelState.AddModelError("", message);
                return View(model);
            }
        }





        public IActionResult AccessDenied()
        {
            return View();
        }


        // dùng cho trang admin để đăng xuất 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogoutAdmin()
        {
            // Xóa tất cả cookie
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // Chuyển hướng về trang chủ
            return RedirectToAction("Index", "Home");
        }

    }
}
