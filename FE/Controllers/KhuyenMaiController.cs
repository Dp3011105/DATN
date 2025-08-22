using FE.Models;
using FE.Service;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
namespace FE.Controllers// controler dùng để thực hiện các chức năng liên quan đến khuyến mãi, crud quản lý khuyến mãi
{
    public class KhuyenMaiController : Controller
    {
        private readonly IKhuyenMaiService _khuyenMaiService;
        private readonly IProductService _productService;

        public KhuyenMaiController(IKhuyenMaiService khuyenMaiService, IProductService productService)
        {
            _khuyenMaiService = khuyenMaiService;
            _productService = productService;
        }

        // GET: /KhuyenMai
        public async Task<IActionResult> Index()
        {
            try
            {
                var khuyenMais = await _khuyenMaiService.GetAllPromotionsAsync();
                return View(khuyenMais);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<FE.Models.KhuyenMaiCrud>());
            }
        }

        public async Task<IActionResult> Indexsp()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

    }
}
