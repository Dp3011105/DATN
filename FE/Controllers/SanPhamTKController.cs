using FE.Filters;
using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [RedirectBasedOnLoginFilter]
    public class SanPhamTKController : Controller
    {
        private readonly IProductService _productService;
        public SanPhamTKController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductViewModel
            {
                AllProducts = await _productService.GetAllProductsAsync()
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
