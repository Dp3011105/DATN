using FE.Filters;
using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{// controller này dùng để thực hiện các chức năng liên quan đến khuyến mãi sản phẩm,thêm khuyến mãi
 // cho sản phẩm , hủy áp dụng khuyến mãi cho sản phẩm
    [RoleAuthorize(2)]// Phương thức này đươc để trong thư mục Filters nhé ae

    public class KhuyenMaiSanPhamController : Controller
    {
        private readonly IProductService _productService;


        public KhuyenMaiSanPhamController(IProductService productService )
        {
            _productService = productService;

        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);  // Truyền model vào view, không redirect
        }
    }
}
