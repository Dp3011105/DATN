using FE.Filters;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using FE.Models;


namespace FE.Controllers
{
    [RedirectBasedOnLoginFilter] // Phương thức này đươc để trong thư mục Filters nhé ae , dùng để chuyển hướng nếu người dùng chưa đăng nhập thi /home , đăng nhập rồi thì /homekhachhang
    public class HomeKhachHangController : Controller
    {
        private readonly IProductService _productService;

        public HomeKhachHangController(IProductService productService)
        {
            _productService = productService;
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
