using FE.Filters;
using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [RoleAuthorize(2)] // Trang cho phép cả vai trò 2 
                          // Phương thức này đươc để trong thư mục Filters nhé ae
    public class ProductDetailsController : Controller
    {
        private readonly IProductDetailsService _productService;

        public ProductDetailsController(IProductDetailsService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int doNgotActivePage = 1, int doNgotInactivePage = 1, int sizeActivePage = 1, int sizeInactivePage = 1, int toppingActivePage = 1, int toppingInactivePage = 1, int luongDaActivePage = 1, int luongDaInactivePage = 1)
        {
            ViewBag.DoNgotActivePage = doNgotActivePage;
            ViewBag.DoNgotInactivePage = doNgotInactivePage;
            ViewBag.SizeActivePage = sizeActivePage;
            ViewBag.SizeInactivePage = sizeInactivePage;
            ViewBag.ToppingActivePage = toppingActivePage;
            ViewBag.ToppingInactivePage = toppingInactivePage;
            ViewBag.LuongDaActivePage = luongDaActivePage;
            ViewBag.LuongDaInactivePage = luongDaInactivePage;
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