using FE.Service.IService;
using FE.Models;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class ProductDetailsController : Controller
    {

        private readonly IProductDetailsService _productService;

        public ProductDetailsController(IProductDetailsService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new
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
