using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    //Trang dành cho Khách hàng không có tài khoản đăng nhập
    public class SanPhamCKController : Controller
    {
        private readonly IProductService _productService;
        public SanPhamCKController(IProductService productService)
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
