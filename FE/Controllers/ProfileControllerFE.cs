using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FE.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        private LoginResponse GetUserData()
        {
            var userDataJson = Request.Cookies["UserData"];
            if (string.IsNullOrEmpty(userDataJson))
                return null;

            return JsonSerializer.Deserialize<LoginResponse>(userDataJson);
        }

        private async Task UpdateUserNameCookie(int khachHangId)
        {
            try
            {
                var profile = await _profileService.GetProfileAsync(khachHangId);
                if (profile != null && !string.IsNullOrEmpty(profile.Ho_Ten))
                {
                    Response.Cookies.Append("UserName", profile.Ho_Ten, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(24)
                    });
                }
            }
            catch
            {
                // Không làm gì nếu lỗi
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userData = GetUserData();
            if (userData == null || !userData.ID_Khach_Hang.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = await _profileService.GetProfileAsync(userData.ID_Khach_Hang.Value);
            if (profile == null)
            {
                ModelState.AddModelError("", "Không thể tải thông tin cá nhân.");
                return View(new ProfileModel());
            }

            // Cập nhật tên user vào cookie khi vào trang profile
            await UpdateUserNameCookie(userData.ID_Khach_Hang.Value);

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileUpdateModel model)
        {
            var userData = GetUserData();
            if (userData == null || !userData.ID_Khach_Hang.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var success = await _profileService.UpdateProfileAsync(userData.ID_Khach_Hang.Value, model);
                if (success)
                {
                    // Cập nhật lại tên user trong cookie sau khi cập nhật thành công
                    await UpdateUserNameCookie(userData.ID_Khach_Hang.Value);
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật thông tin.";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressCreateModel model)
        {
            var userData = GetUserData();
            if (userData == null || !userData.ID_Khach_Hang.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var success = await _profileService.AddAddressAsync(userData.ID_Khach_Hang.Value, model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Thêm địa chỉ thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể thêm địa chỉ.";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress(AddressModel model)
        {
            var userData = GetUserData();
            if (userData == null || !userData.ID_Khach_Hang.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var success = await _profileService.UpdateAddressAsync(userData.ID_Khach_Hang.Value, model.ID_Dia_Chi, model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Cập nhật địa chỉ thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật địa chỉ.";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userData = GetUserData();
            if (userData == null || !userData.ID_Khach_Hang.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var success = await _profileService.DeleteAddressAsync(userData.ID_Khach_Hang.Value, id);
            if (success)
            {
                TempData["SuccessMessage"] = "Xóa địa chỉ thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa địa chỉ.";
            }

            return RedirectToAction("Index");
        }
    }
}