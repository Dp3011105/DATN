using FE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using FE.Models;

namespace FE.Filters
{
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int[] _allowedRoles;

        public RoleAuthorizeAttribute(params int[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Lấy dữ liệu từ cookie
            var userDataJson = context.HttpContext.Request.Cookies["UserData"];
            if (string.IsNullOrEmpty(userDataJson))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            try
            {
                // Deserialize dữ liệu từ cookie
                var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);

                if (userData?.VaiTros == null)
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                    return;
                }

                // Kiểm tra quyền truy cập
                bool hasAccess = false;

                // Nếu có vai trò 2 hoặc cả 2 và 3 -> cho phép truy cập mọi trang
                if (userData.VaiTros.Contains(2))
                {
                    hasAccess = true;
                }
                // Nếu chỉ có vai trò 3 -> chỉ cho phép truy cập các trang được chỉ định
                else if (userData.VaiTros.Contains(3) && !userData.VaiTros.Contains(2))
                {
                    hasAccess = _allowedRoles.Length == 0 || _allowedRoles.Any(role => userData.VaiTros.Contains(role));
                }

                if (!hasAccess)
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                }
            }
            catch
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
