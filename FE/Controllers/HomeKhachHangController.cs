using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class HomeKhachHangController : Controller
    {
        private readonly IProductService _productService;

        public HomeKhachHangController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
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
