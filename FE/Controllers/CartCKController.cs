using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class CartCKController : Controller
    {
        private readonly IProductService _productService;

        public CartCKController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            // Trang giỏ hàng không cần model phức tạp, chỉ cần view để load script
            return View();
        }
    }
}
