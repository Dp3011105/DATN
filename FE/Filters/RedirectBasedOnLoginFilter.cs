using FE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using FE.Models;

namespace FE.Filters
{
    public class RedirectBasedOnLoginFilter : ActionFilterAttribute // ĐOẠN CODE ĐỂ PHÂN BIỆT ĐĂNG NHẬP CHO 2 HOME KHÁCH HÀNG VÀ HOME KHÁCH VÃNG LAI
    {
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    // Lấy cookie UserData
        //    var userDataJson = context.HttpContext.Request.Cookies["UserData"];
        //    bool isLoggedIn = !string.IsNullOrEmpty(userDataJson);

        //    // Lấy tên controller và action hiện tại
        //    var controllerName = context.RouteData.Values["controller"]?.ToString();
        //    var actionName = context.RouteData.Values["action"]?.ToString();

        //    // Kiểm tra trạng thái đăng nhập và chuyển hướng
        //    if (isLoggedIn)
        //    {
        //        // Nếu đã đăng nhập, không cho phép truy cập /Home, chuyển hướng đến /HomeKhachHang
        //        if (controllerName.Equals("Home", StringComparison.OrdinalIgnoreCase) &&
        //            actionName.Equals("Index", StringComparison.OrdinalIgnoreCase))
        //        {
        //            context.Result = new RedirectToActionResult("Index", "HomeKhachHang", null);
        //        }
        //    }
        //    else
        //    {
        //        // Nếu chưa đăng nhập, không cho phép truy cập /HomeKhachHang, chuyển hướng đến /Home
        //        if (controllerName.Equals("HomeKhachHang", StringComparison.OrdinalIgnoreCase) &&
        //            actionName.Equals("Index", StringComparison.OrdinalIgnoreCase))
        //        {
        //            context.Result = new RedirectToActionResult("Index", "Home", null);
        //        }
        //    }

        //    base.OnActionExecuting(context); // Gọi phương thức cơ sở
        //}






        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy cookie UserData
            var userDataJson = context.HttpContext.Request.Cookies["UserData"];
            bool isLoggedIn = !string.IsNullOrEmpty(userDataJson);

            // Lấy tên controller và action hiện tại
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();

            if (isLoggedIn)
            {
                try
                {
                    // Deserialize dữ liệu từ cookie
                    var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);

                    if (userData?.VaiTros == null)
                    {
                        context.Result = new RedirectToActionResult("Login", "Account", null);
                        return;
                    }

                    // Kiểm tra VaiTro
                    if (!userData.VaiTros.Contains(1)) // Nếu không có vai trò 1 (Khách Hàng)
                    {
                        context.Result = new RedirectToActionResult("Index", "ThongKe", null);
                        return;
                    }

                    // Nếu đã đăng nhập và có vai trò 1, không cho phép truy cập /Home, chuyển hướng đến /HomeKhachHang
                    if (controllerName.Equals("Home", StringComparison.OrdinalIgnoreCase) &&
                        actionName.Equals("Index", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Result = new RedirectToActionResult("Index", "HomeKhachHang", null);
                    }
                }
                catch
                {
                    // Nếu có lỗi khi deserialize cookie, chuyển hướng về Login
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }
            else
            {
                // Nếu chưa đăng nhập, không cho phép truy cập /HomeKhachHang, chuyển hướng đến /Home
                if (controllerName.Equals("HomeKhachHang", StringComparison.OrdinalIgnoreCase) &&
                    actionName.Equals("Index", StringComparison.OrdinalIgnoreCase))
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }

            base.OnActionExecuting(context); // Gọi phương thức cơ sở
        }





    }
}
