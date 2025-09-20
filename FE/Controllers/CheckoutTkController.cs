//using FE.Models;
//using FE.Service.IService;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading.Tasks;
//namespace FE.Controllers
//{
//    public class CheckoutTkController : Controller
//    {
//        private readonly ICheckoutService _checkoutService;
//        private readonly HttpClient _httpClient;
//        public CheckoutTkController(ICheckoutService checkoutService, HttpClient httpClient)
//        {
//            _checkoutService = checkoutService;
//            _httpClient = httpClient;
//        }


//        public async Task<IActionResult> Index()
//        {
//            var userDataJson = HttpContext.Request.Cookies["UserData"];
//            if (string.IsNullOrEmpty(userDataJson))
//            {
//                Console.WriteLine("Cookie UserData không tồn tại.");
//                TempData["Error"] = "Vui lòng đăng nhập để tiếp tục thanh toán.";
//                return RedirectToAction("Login", "Account");
//            }

//            try
//            {
//                var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);
//                if (userData == null || userData.ID_Khach_Hang == null || !userData.VaiTros.Contains(1))
//                {
//                    Console.WriteLine($"ID_Khach_Hang không hợp lệ hoặc vai trò không phù hợp: ID_Khach_Hang={userData?.ID_Khach_Hang}, VaiTros={string.Join(",", userData?.VaiTros ?? new List<int>())}");
//                    TempData["Error"] = "Bạn không có quyền truy cập trang thanh toán. Vui lòng đăng nhập bằng tài khoản khách hàng.";
//                    return RedirectToAction("Login", "Account");
//                }

//                var customerId = userData.ID_Khach_Hang.ToString();
//                Console.WriteLine($"ID_Khach_Hang: {customerId}");
//                ViewData["CustomerId"] = customerId;

//                // Lấy dữ liệu từ cookie selectedCartItems
//                var selectedItemsJson = HttpContext.Request.Cookies["selectedCartItems"];
//                if (string.IsNullOrEmpty(selectedItemsJson))
//                {
//                    Console.WriteLine("Cookie selectedCartItems rỗng hoặc không tồn tại.");
//                    TempData["Error"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.";
//                    return RedirectToAction("Index", "Homekhachhang");
//                }

//                List<ChiTietGioHangCheckOutTK> cartItems;
//                try
//                {
//                    // Decode và deserialize cookie
//                    selectedItemsJson = WebUtility.UrlDecode(selectedItemsJson);
//                    Console.WriteLine($"selectedCartItems sau decode: {selectedItemsJson}");

//                    cartItems = JsonSerializer.Deserialize<List<ChiTietGioHangCheckOutTK>>(selectedItemsJson, new JsonSerializerOptions
//                    {
//                        PropertyNameCaseInsensitive = true
//                    });
//                    Console.WriteLine($"Deserialize thành công selectedCartItems, số mục: {cartItems?.Count ?? 0}");
//                }
//                catch (JsonException ex)
//                {
//                    Console.WriteLine($"Lỗi deserialize selectedCartItems: {ex.Message}, Input: {selectedItemsJson}");
//                    TempData["Error"] = "Dữ liệu sản phẩm được chọn không hợp lệ. Vui lòng kiểm tra và thử lại.";
//                    return RedirectToAction("Index", "Homekhachhang");
//                }

//                if (cartItems == null || !cartItems.Any())
//                {
//                    Console.WriteLine("Danh sách mục từ cookie rỗng hoặc không hợp lệ.");
//                    TempData["Error"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.";
//                    return RedirectToAction("Index", "Homekhachhang");
//                }

//                // Kiểm tra dữ liệu hợp lệ và xử lý khuyến mãi
//                foreach (var item in cartItems)
//                {
//                    if (item.ID_GioHang_ChiTiet <= 0 || string.IsNullOrEmpty(item.Ten_San_Pham))
//                    {
//                        Console.WriteLine($"Mục không hợp lệ trong cookie: ID_GioHang_ChiTiet={item.ID_GioHang_ChiTiet}, Ten_San_Pham={item.Ten_San_Pham}");
//                        TempData["Error"] = "Dữ liệu sản phẩm được chọn không hợp lệ. Vui lòng kiểm tra và thử lại.";
//                        return RedirectToAction("Index", "Homekhachhang");
//                    }
//                    // Kiểm tra khuyến mãi hợp lệ
//                    if (item.Khuyen_Mai != null && (string.IsNullOrEmpty(item.Khuyen_Mai.Ten_Khuyen_Mai) || item.Khuyen_Mai.Gia_Giam <= 0))
//                    {
//                        Console.WriteLine($"Khuyến mãi không hợp lệ cho mục: ID_GioHang_ChiTiet={item.ID_GioHang_ChiTiet}, Ten_Khuyen_Mai={item.Khuyen_Mai.Ten_Khuyen_Mai}, Gia_Giam={item.Khuyen_Mai.Gia_Giam}");
//                        item.Khuyen_Mai = null; // Đặt lại khuyến mãi nếu không hợp lệ
//                        item.Gia_Hien_Thi = item.Gia_Goc;
//                    }
//                    Console.WriteLine($"Mục trong cookie: ID_GioHang_ChiTiet={item.ID_GioHang_ChiTiet}, Ten_San_Pham={item.Ten_San_Pham}, So_Luong={item.So_Luong}, Khuyen_Mai={JsonSerializer.Serialize(item.Khuyen_Mai)}, Toppings={JsonSerializer.Serialize(item.Toppings ?? new List<ToppingCheckOutTK>())}");
//                }

//                // Lấy dữ liệu địa chỉ, voucher, phương thức thanh toán từ service
//                var paymentMethods = await _checkoutService.GetPaymentMethodsAsync();
//                var addresses = await _checkoutService.GetCustomerAddressesAsync(customerId);
//                var vouchers = await _checkoutService.GetVouchersAsync(customerId);

//                // Truyền dữ liệu từ cookie vào ViewBag
//                ViewBag.SelectedItems = selectedItemsJson;

//                var model = new CheckoutTkViewModel
//                {
//                    PaymentMethods = paymentMethods ?? new List<HinhThucThanhToanCheckOutTK>(),
//                    Addresses = addresses ?? new List<DiaChiCheckOutTK>(),
//                    Vouchers = vouchers ?? new List<VoucherCheckOutTK>(),
//                    CartItems = cartItems
//                };

//                return View(model);
//            }
//            catch (JsonException ex)
//            {
//                Console.WriteLine($"Lỗi deserialize UserData: {ex.Message}, Input: {userDataJson}");
//                TempData["Error"] = "Dữ liệu người dùng không hợp lệ. Vui lòng đăng nhập lại.";
//                return RedirectToAction("Login", "Account");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi không xác định trong Checkout: {ex.Message}, StackTrace: {ex.StackTrace}");
//                TempData["Error"] = "Đã xảy ra lỗi khi tải trang thanh toán. Vui lòng thử lại.";
//                return RedirectToAction("Index", "Homekhachhang");
//            }
//        }






//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddAddress(DiaChiCheckOutTK newAddress)
//        {
//            var userDataJson = HttpContext.Request.Cookies["UserData"];
//            if (string.IsNullOrEmpty(userDataJson))
//            {
//                Console.WriteLine("Cookie UserData không tồn tại khi thêm địa chỉ.");
//                return Unauthorized("Vui lòng đăng nhập để thêm địa chỉ.");
//            }

//            try
//            {
//                var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);
//                if (userData == null || userData.ID_Khach_Hang == null || !userData.VaiTros.Contains(1))
//                {
//                    Console.WriteLine($"ID_Khach_Hang không hợp lệ hoặc vai trò không phù hợp khi thêm địa chỉ: ID_Khach_Hang={userData?.ID_Khach_Hang}");
//                    return Unauthorized("Bạn không có quyền thêm địa chỉ. Vui lòng đăng nhập bằng tài khoản khách hàng.");
//                }

//                var customerId = userData.ID_Khach_Hang.ToString();
//                var success = await _checkoutService.AddAddressAsync(customerId, newAddress);
//                if (success)
//                {
//                    TempData["Success"] = "Thêm địa chỉ thành công.";
//                    return RedirectToAction("Index");
//                }
//                TempData["Error"] = "Không thể thêm địa chỉ.";
//                return RedirectToAction("Index");
//            }
//            catch (JsonException ex)
//            {
//                Console.WriteLine($"Lỗi khi deserialize UserData trong AddAddress: {ex.Message}");
//                return Unauthorized("Dữ liệu người dùng không hợp lệ.");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi khi thêm địa chỉ: {ex.Message}");
//                TempData["Error"] = "Đã xảy ra lỗi khi thêm địa chỉ.";
//                return RedirectToAction("Index");
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UpdateAddress(DiaChiCheckOutTK updatedAddress)
//        {
//            var userDataJson = HttpContext.Request.Cookies["UserData"];
//            if (string.IsNullOrEmpty(userDataJson))
//            {
//                Console.WriteLine("Cookie UserData không tồn tại khi cập nhật địa chỉ.");
//                return Unauthorized("Vui lòng đăng nhập để cập nhật địa chỉ.");
//            }

//            try
//            {
//                var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);
//                if (userData == null || userData.ID_Khach_Hang == null || !userData.VaiTros.Contains(1))
//                {
//                    Console.WriteLine($"ID_Khach_Hang không hợp lệ hoặc vai trò không phù hợp khi cập nhật địa chỉ: ID_Khach_Hang={userData?.ID_Khach_Hang}");
//                    return Unauthorized("Bạn không có quyền cập nhật địa chỉ. Vui lòng đăng nhập bằng tài khoản khách hàng.");
//                }

//                var customerId = userData.ID_Khach_Hang.ToString();
//                var success = await _checkoutService.UpdateAddressAsync(customerId, updatedAddress);
//                if (success)
//                {
//                    TempData["Success"] = "Cập nhật địa chỉ thành công.";
//                    return RedirectToAction("Index");
//                }
//                TempData["Error"] = "Không thể cập nhật địa chỉ.";
//                return RedirectToAction("Index");
//            }
//            catch (JsonException ex)
//            {
//                Console.WriteLine($"Lỗi khi deserialize UserData trong UpdateAddress: {ex.Message}");
//                return Unauthorized("Dữ liệu người dùng không hợp lệ.");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi khi cập nhật địa chỉ: {ex.Message}");
//                TempData["Error"] = "Đã xảy ra lỗi khi cập nhật địa chỉ.";
//                return RedirectToAction("Index");
//            }
//        }



//        [HttpGet("checkouttk/vnpaydone")]
//        public IActionResult VnpayDone(string orderId, string transactionId, bool success, string code)
//        {
//            var model = new PaymentResultViewModel
//            {
//                OrderId = orderId,
//                TransactionId = transactionId,
//                Success = success,
//                VnPayResponseCode = code
//            };

//            return View(model);
//        }




//    }



//    public class PaymentResultViewModel
//    {
//        public string OrderId { get; set; }
//        public string TransactionId { get; set; }
//        public bool Success { get; set; }
//        public string VnPayResponseCode { get; set; }
//        public string Message { get; set; } // thêm trường mô tả trạng thái

//    }



//}





using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
namespace FE.Controllers
{
    public class CheckoutTkController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        private readonly HttpClient _httpClient;
        public CheckoutTkController(ICheckoutService checkoutService, HttpClient httpClient)
        {
            _checkoutService = checkoutService;
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index()
        {
            var userDataJson = HttpContext.Request.Cookies["UserData"];
            if (string.IsNullOrEmpty(userDataJson))
            {
                Console.WriteLine("Cookie UserData không tồn tại.");
                TempData["Error"] = "Vui lòng đăng nhập để tiếp tục thanh toán.";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);
                if (userData == null || userData.ID_Khach_Hang == null || !userData.VaiTros.Contains(1))
                {
                    Console.WriteLine($"ID_Khach_Hang không hợp lệ hoặc vai trò không phù hợp: ID_Khach_Hang={userData?.ID_Khach_Hang}, VaiTros={string.Join(",", userData?.VaiTros ?? new List<int>())}");
                    TempData["Error"] = "Bạn không có quyền truy cập trang thanh toán. Vui lòng đăng nhập bằng tài khoản khách hàng.";
                    return RedirectToAction("Login", "Account");
                }

                var customerId = userData.ID_Khach_Hang.ToString();
                Console.WriteLine($"ID_Khach_Hang: {customerId}");
                ViewData["CustomerId"] = customerId;

                // Lấy dữ liệu từ cookie selectedCartItems
                var selectedItemsJson = HttpContext.Request.Cookies["selectedCartItems"];
                if (string.IsNullOrEmpty(selectedItemsJson))
                {
                    Console.WriteLine("Cookie selectedCartItems rỗng hoặc không tồn tại.");
                    TempData["Error"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.";
                    return RedirectToAction("Index", "Homekhachhang");
                }

                List<ChiTietGioHangCheckOutTK> cartItems;
                try
                {
                    // Decode và deserialize cookie
                    selectedItemsJson = WebUtility.UrlDecode(selectedItemsJson);
                    Console.WriteLine($"selectedCartItems sau decode: {selectedItemsJson}");

                    cartItems = JsonSerializer.Deserialize<List<ChiTietGioHangCheckOutTK>>(selectedItemsJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    Console.WriteLine($"Deserialize thành công selectedCartItems, số mục: {cartItems?.Count ?? 0}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Lỗi deserialize selectedCartItems: {ex.Message}, Input: {selectedItemsJson}");
                    TempData["Error"] = "Dữ liệu sản phẩm được chọn không hợp lệ. Vui lòng kiểm tra và thử lại.";
                    return RedirectToAction("Index", "Homekhachhang");
                }

                if (cartItems == null || !cartItems.Any())
                {
                    Console.WriteLine("Danh sách mục từ cookie rỗng hoặc không hợp lệ.");
                    TempData["Error"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.";
                    return RedirectToAction("Index", "Homekhachhang");
                }

                // Kiểm tra dữ liệu hợp lệ và xử lý khuyến mãi
                foreach (var item in cartItems)
                {
                    if (item.ID_GioHang_ChiTiet <= 0 || string.IsNullOrEmpty(item.Ten_San_Pham))
                    {
                        Console.WriteLine($"Mục không hợp lệ trong cookie: ID_GioHang_ChiTiet={item.ID_GioHang_ChiTiet}, Ten_San_Pham={item.Ten_San_Pham}");
                        TempData["Error"] = "Dữ liệu sản phẩm được chọn không hợp lệ. Vui lòng kiểm tra và thử lại.";
                        return RedirectToAction("Index", "Homekhachhang");
                    }
                    // Kiểm tra khuyến mãi hợp lệ
                    if (item.Khuyen_Mai != null && (string.IsNullOrEmpty(item.Khuyen_Mai.Ten_Khuyen_Mai) || item.Khuyen_Mai.Gia_Giam <= 0))
                    {
                        Console.WriteLine($"Khuyến mãi không hợp lệ cho mục: ID_GioHang_ChiTiet={item.ID_GioHang_ChiTiet}, Ten_Khuyen_Mai={item.Khuyen_Mai.Ten_Khuyen_Mai}, Gia_Giam={item.Khuyen_Mai.Gia_Giam}");
                        item.Khuyen_Mai = null; // Đặt lại khuyến mãi nếu không hợp lệ
                        item.Gia_Hien_Thi = item.Gia_Goc;
                    }
                    Console.WriteLine($"Mục trong cookie: ID_GioHang_ChiTiet={item.ID_GioHang_ChiTiet}, Ten_San_Pham={item.Ten_San_Pham}, So_Luong={item.So_Luong}, Khuyen_Mai={JsonSerializer.Serialize(item.Khuyen_Mai)}, Toppings={JsonSerializer.Serialize(item.Toppings ?? new List<ToppingCheckOutTK>())}");
                }

                // Lấy dữ liệu địa chỉ, voucher, phương thức thanh toán từ service
                var paymentMethods = await _checkoutService.GetPaymentMethodsAsync();
                var addresses = await _checkoutService.GetCustomerAddressesAsync(customerId);
                var vouchers = await _checkoutService.GetVouchersAsync(customerId);

                // Truyền dữ liệu từ cookie vào ViewBag
                ViewBag.SelectedItems = selectedItemsJson;

                var model = new CheckoutTkViewModel
                {
                    PaymentMethods = paymentMethods ?? new List<HinhThucThanhToanCheckOutTK>(),
                    Addresses = addresses ?? new List<DiaChiCheckOutTK>(),
                    Vouchers = vouchers ?? new List<VoucherCheckOutTK>(),
                    CartItems = cartItems
                };

                return View(model);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Lỗi deserialize UserData: {ex.Message}, Input: {userDataJson}");
                TempData["Error"] = "Dữ liệu người dùng không hợp lệ. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định trong Checkout: {ex.Message}, StackTrace: {ex.StackTrace}");
                TempData["Error"] = "Đã xảy ra lỗi khi tải trang thanh toán. Vui lòng thử lại.";
                return RedirectToAction("Index", "Homekhachhang");
            }
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(DiaChiCheckOutTK newAddress)
        {
            var userDataJson = HttpContext.Request.Cookies["UserData"];
            if (string.IsNullOrEmpty(userDataJson))
            {
                Console.WriteLine("Cookie UserData không tồn tại khi thêm địa chỉ.");
                return Unauthorized("Vui lòng đăng nhập để thêm địa chỉ.");
            }

            try
            {
                var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);
                if (userData == null || userData.ID_Khach_Hang == null || !userData.VaiTros.Contains(1))
                {
                    Console.WriteLine($"ID_Khach_Hang không hợp lệ hoặc vai trò không phù hợp khi thêm địa chỉ: ID_Khach_Hang={userData?.ID_Khach_Hang}");
                    return Unauthorized("Bạn không có quyền thêm địa chỉ. Vui lòng đăng nhập bằng tài khoản khách hàng.");
                }

                var customerId = userData.ID_Khach_Hang.ToString();
                var success = await _checkoutService.AddAddressAsync(customerId, newAddress);
                if (success)
                {
                    TempData["Success"] = "Thêm địa chỉ thành công.";
                    return RedirectToAction("Index");
                }
                TempData["Error"] = "Không thể thêm địa chỉ.";
                return RedirectToAction("Index");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Lỗi khi deserialize UserData trong AddAddress: {ex.Message}");
                return Unauthorized("Dữ liệu người dùng không hợp lệ.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm địa chỉ: {ex.Message}");
                TempData["Error"] = "Đã xảy ra lỗi khi thêm địa chỉ.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(DiaChiCheckOutTK updatedAddress)
        {
            var userDataJson = HttpContext.Request.Cookies["UserData"];
            if (string.IsNullOrEmpty(userDataJson))
            {
                Console.WriteLine("Cookie UserData không tồn tại khi cập nhật địa chỉ.");
                return Unauthorized("Vui lòng đăng nhập để cập nhật địa chỉ.");
            }

            try
            {
                var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);
                if (userData == null || userData.ID_Khach_Hang == null || !userData.VaiTros.Contains(1))
                {
                    Console.WriteLine($"ID_Khach_Hang không hợp lệ hoặc vai trò không phù hợp khi cập nhật địa chỉ: ID_Khach_Hang={userData?.ID_Khach_Hang}");
                    return Unauthorized("Bạn không có quyền cập nhật địa chỉ. Vui lòng đăng nhập bằng tài khoản khách hàng.");
                }

                var customerId = userData.ID_Khach_Hang.ToString();
                var success = await _checkoutService.UpdateAddressAsync(customerId, updatedAddress);
                if (success)
                {
                    TempData["Success"] = "Cập nhật địa chỉ thành công.";
                    return RedirectToAction("Index");
                }
                TempData["Error"] = "Không thể cập nhật địa chỉ.";
                return RedirectToAction("Index");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Lỗi khi deserialize UserData trong UpdateAddress: {ex.Message}");
                return Unauthorized("Dữ liệu người dùng không hợp lệ.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật địa chỉ: {ex.Message}");
                TempData["Error"] = "Đã xảy ra lỗi khi cập nhật địa chỉ.";
                return RedirectToAction("Index");
            }
        }



        [HttpGet("checkouttk/vnpaydone")]
        public IActionResult VnpayDone(string orderId, string transactionId, bool success, string code)
        {
            var model = new PaymentResultViewModel
            {
                OrderId = orderId,
                TransactionId = transactionId,
                Success = success,
                VnPayResponseCode = code
            };

            return View(model);
        }




    }



    public class PaymentResultViewModel
    {
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public bool Success { get; set; }
        public string VnPayResponseCode { get; set; }
        public string Message { get; set; } // thêm trường mô tả trạng thái

    }



}

