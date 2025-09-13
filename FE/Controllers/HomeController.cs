using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using FE.Filters;

namespace FE.Controllers
{
    [RedirectBasedOnLoginFilter]  // Phương thức này đươc để trong thư mục Filters nhé ae , dùng để chuyển hướng nếu người dùng chưa đăng nhập thi /home , đăng nhập rồi thì /homekhachhang
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(  IProductService productService, ILogger<HomeController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var products = await _productService.GetAllProductsAsync();
        //    return View(products);
        //}


        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductViewModel
            {
                AllProducts = await _productService.GetAllProductsAsync(),
                MostPurchasedProducts = await _productService.GetMostPurchasedProductsAsync()
            };
            return View(viewModel);
        }


        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

       
    }
}