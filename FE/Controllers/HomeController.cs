using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
//Ở ĐÂY THỰC HIỆN HIỂN THỊ DANH SÁCH SẢN PHẨM , CHI TIẾT SẢN PHẨM , GIỎ HÀNG VÀ CÁC CHỨC NĂNG LIÊN QUAN ĐẾN SẢN PHẨM
//(Phước)   
namespace FE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IProductService _productService;
        private readonly ICartService _cartService;


        public HomeController(ICartService cartService,IProductService productService, ILogger<HomeController> logger)
        {
            _cartService = cartService;
            _productService = productService;
            _logger = logger;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        


        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();

            // Lấy UserData từ cookie
            var userDataJson = Request.Cookies["UserData"];
            int? userId = null;
            if (!string.IsNullOrEmpty(userDataJson))
            {
                var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);
                if (userData?.ID_Khach_Hang.HasValue == true)
                {
                    var cart = await _cartService.GetCartByUserIdAsync(userData.ID_Khach_Hang.Value);
                    ViewBag.Cart = cart;
                    userId = userData.ID_Khach_Hang.Value; // Pass cho JS xóa
                }
            }
            ViewBag.UserId = userId;

            return View(products);
        }




        // GET: Lấy chi tiết sản phẩm theo ID
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
