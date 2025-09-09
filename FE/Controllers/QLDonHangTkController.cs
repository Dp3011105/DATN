using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;


namespace FE.Controllers
{
    public class QLDonHangTkController : Controller
    {
        private readonly IQLDonHangTkService _service;

        public QLDonHangTkController(IQLDonHangTkService service)
        {
            _service = service;
        }

        //public async Task<IActionResult> Index(string status = null)
        //{
        //    var customerId = GetCustomerId();
        //    if (customerId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var orders = await _service.GetOrdersAsync(customerId.Value);

        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        orders = orders.Where(o => o.Trang_Thai == status).ToList();
        //    }

        //    ViewBag.CurrentStatus = status;
        //    return View(orders);
        //}


        //public async Task<IActionResult> Details(int id)
        //{
        //    var customerId = GetCustomerId();
        //    if (customerId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var orders = await _service.GetOrdersAsync(customerId.Value);
        //    var order = orders.FirstOrDefault(o => o.ID_Hoa_Don == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}


        //[HttpPost]
        //public async Task<IActionResult> CancelOrder(int idHoaDon, string lyDoHuyDon)
        //{
        //    var customerId = GetCustomerId();
        //    if (customerId == null)
        //    {
        //        return Json(new { success = false, message = "Vui lòng đăng nhập lại." });
        //    }

        //    var request = new DonHangTKCancelRequest
        //    {
        //        ID_Hoa_Don = idHoaDon,
        //        ID_Khach_Hang = customerId.Value,
        //        LyDoHuyDon = lyDoHuyDon
        //    };

        //    var success = await _service.CancelOrderAsync(request);
        //    return Json(new { success, message = success ? "Hủy đơn hàng thành công." : "Hủy đơn hàng thất bại." });
        //}

        //private int? GetCustomerId()
        //{
        //    var userData = Request.Cookies["UserData"];
        //    if (userData != null)
        //    {
        //        try
        //        {
        //            var parsedData = JsonSerializer.Deserialize<Dictionary<string, object>>(userData);
        //            if (parsedData.ContainsKey("ID_Khach_Hang") && int.TryParse(parsedData["ID_Khach_Hang"].ToString(), out var id))
        //            {
        //                return id;
        //            }
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //    return null;
        //}





        public async Task<IActionResult> Index(string status = null)
        {
            var customerId = GetCustomerId();
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _service.GetOrdersAsync(customerId.Value);
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.Trang_Thai == status).ToList();
            }

            ViewBag.CurrentStatus = status;
            ViewBag.ID_Khach_Hang = customerId.Value; // Truyền ID_Khach_Hang vào ViewBag
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customerId = GetCustomerId();
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _service.GetOrdersAsync(customerId.Value);
            var order = orders.FirstOrDefault(o => o.ID_Hoa_Don == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int idHoaDon, string lyDoHuyDon)
        {
            var customerId = GetCustomerId();
            if (customerId == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập lại." });
            }

            var request = new DonHangTKCancelRequest
            {
                ID_Hoa_Don = idHoaDon,
                ID_Khach_Hang = customerId.Value,
                LyDoHuyDon = lyDoHuyDon
            };

            var success = await _service.CancelOrderAsync(request);
            return Json(new { success, message = success ? "Hủy đơn hàng thành công." : "Hủy đơn hàng thất bại." });
        }

        private int? GetCustomerId()
        {
            var userData = Request.Cookies["UserData"];
            if (userData != null)
            {
                try
                {
                    var parsedData = JsonSerializer.Deserialize<Dictionary<string, object>>(userData);
                    if (parsedData.ContainsKey("ID_Khach_Hang") && int.TryParse(parsedData["ID_Khach_Hang"].ToString(), out var id))
                    {
                        return id;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }



    }
}
