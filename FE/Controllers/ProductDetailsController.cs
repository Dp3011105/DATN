using FE.Filters;
using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [RoleAuthorize(2, 3)] // Trang cho phép cả vai trò 2 và 3
                          // Phương thức này đươc để trong thư mục Filters nhé ae
    public class ProductDetailsController : Controller
    {
        private readonly IProductDetailsService _productService;

        public ProductDetailsController(IProductDetailsService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductDetailViewModel
            {
                DoNgots = await _productService.GetDoNgotsAsync(),
                Sizes = await _productService.GetSizesAsync(),
                Toppings = await _productService.GetToppingsAsync(),
                LuongDas = await _productService.GetLuongDasAsync()
            };
            return View(viewModel);
        }
    }
}