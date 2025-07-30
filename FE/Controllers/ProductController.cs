using FE.Models;
using FE.Service;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using FE.Service;

namespace FE.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService sanPhamService)
        {
            _productService = sanPhamService;
        }

        // GET: Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // GET: Lấy chi tiết sản phẩm theo ID
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> AddProduct()
        {
            ViewBag.Sizes = await _productService.GetSizesAsync();
            ViewBag.Toppings = await _productService.GetToppingsAsync();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync();
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync();

            return View(new AddProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.AddProductAsync(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Sizes = await _productService.GetSizesAsync();
            ViewBag.Toppings = await _productService.GetToppingsAsync();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync();
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync();

            return View(model);
        }



        // GET: Hiển thị form sửa sản phẩm
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new UpdateProductViewModel
            {
                ID_San_Pham = product.ID_San_Pham,
                TenSanPham = product.Ten_San_Pham,
                Gia = product.Gia,
                SoLuong = product.So_Luong,
                MoTa = product.Mo_Ta,
                CurrentImagePath = product.Hinh_Anh, // Lưu đường dẫn hình ảnh hiện tại
                SelectedSizes = product.Sizes?.Select(s => s.ID_Size).ToList() ?? new List<int>(),
                SelectedToppings = product.Toppings?.Select(t => t.ID_Topping).ToList() ?? new List<int>(),
                SelectedLuongDas = product.LuongDas?.Select(l => l.ID_LuongDa).ToList() ?? new List<int>(),
                SelectedDoNgots = product.DoNgots?.Select(d => d.ID_DoNgot).ToList() ?? new List<int>()
            };

            ViewBag.Sizes = await _productService.GetSizesAsync();
            ViewBag.Toppings = await _productService.GetToppingsAsync();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync();
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync();

            return View(model);
        }

        // POST: Cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> EditProduct(UpdateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProductAsync(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Sizes = await _productService.GetSizesAsync();
            ViewBag.Toppings = await _productService.GetToppingsAsync();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync();
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync();

            return View(model);
        }


    }
}

