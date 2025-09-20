using FE.Filters;
using FE.Models;
using FE.Service;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FE.Controllers
{
    [RoleAuthorize(2, 3)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService sanPhamService)
        {
            _productService = sanPhamService;
        }

        // GET: Hiển thị danh sách sản phẩm
        //public async Task<IActionResult> Index(string searchTerm = "", bool? statusFilter = null, bool? promotionFilter = null, string quantityFilter = null)
        //{
        //    var products = await _productService.GetAllProductsAsync();
        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        products = products.Where(p => p.Ten_San_Pham?.ToLower().Contains(searchTerm.ToLower()) ?? false).ToList();
        //    }
        //    if (statusFilter.HasValue)
        //    {
        //        products = products.Where(p => p.Trang_Thai == statusFilter.Value).ToList();
        //    }
        //    if (promotionFilter.HasValue)
        //    {
        //        var currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        //        if (promotionFilter.Value)
        //        {
        //            products = products.Where(p => p.KhuyenMais != null && p.KhuyenMais.Any(km => currentDate >= km.Ngay_Bat_Dau && currentDate <= km.Ngay_Ket_Thuc)).ToList();
        //        }
        //        else
        //        {
        //            products = products.Where(p => p.KhuyenMais == null || !p.KhuyenMais.Any(km => currentDate >= km.Ngay_Bat_Dau && currentDate <= km.Ngay_Ket_Thuc)).ToList();
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(quantityFilter))
        //    {
        //        switch (quantityFilter.ToLower())
        //        {
        //            case "under-100":
        //                products = products.Where(p => p.So_Luong < 3).ToList();
        //                break;
        //            case "over-1000":
        //                products = products.Where(p => p.So_Luong > 100).ToList();
        //                break;
        //        }
        //    }
        //    ViewBag.SearchTerm = searchTerm;
        //    ViewBag.StatusFilter = statusFilter;
        //    ViewBag.PromotionFilter = promotionFilter;
        //    ViewBag.QuantityFilter = quantityFilter;
        //    return View(products);
        //}
        public async Task<IActionResult> Index(string searchTerm = "", bool? statusFilter = null, bool? promotionFilter = null, string quantityFilter = null, int activePage = 1, int inactivePage = 1)
        {
            var products = await _productService.GetAllProductsAsync();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Ten_San_Pham?.ToLower().Contains(searchTerm.ToLower()) ?? false).ToList();
            }
            if (statusFilter.HasValue)
            {
                products = products.Where(p => p.Trang_Thai == statusFilter.Value).ToList();
            }
            if (promotionFilter.HasValue)
            {
                var currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                if (promotionFilter.Value)
                {
                    products = products.Where(p => p.KhuyenMais != null && p.KhuyenMais.Any(km => currentDate >= km.Ngay_Bat_Dau && currentDate <= km.Ngay_Ket_Thuc)).ToList();
                }
                else
                {
                    products = products.Where(p => p.KhuyenMais == null || !p.KhuyenMais.Any(km => currentDate >= km.Ngay_Bat_Dau && currentDate <= km.Ngay_Ket_Thuc)).ToList();
                }
            }
            if (!string.IsNullOrEmpty(quantityFilter))
            {
                switch (quantityFilter.ToLower())
                {
                    case "under-100":
                        products = products.Where(p => p.So_Luong < 3).ToList();
                        break;
                    case "over-1000":
                        products = products.Where(p => p.So_Luong > 100).ToList();
                        break;
                }
            }
            ViewBag.SearchTerm = searchTerm;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.PromotionFilter = promotionFilter;
            ViewBag.QuantityFilter = quantityFilter;
            ViewBag.ActivePage = activePage;
            ViewBag.InactivePage = inactivePage;
            return View(products);
        }
        // GET: Lấy chi tiết sản phẩm theo ID
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại.";
                return NotFound();
            }
            return View(product);
        }

        //// GET: Hiển thị form thêm sản phẩm
        //public async Task<IActionResult> AddProduct()
        //{
        //    ViewBag.Sizes = await _productService.GetSizesAsync();
        //    ViewBag.Toppings = await _productService.GetToppingsAsync();
        //    ViewBag.LuongDas = await _productService.GetLuongDasAsync();
        //    ViewBag.DoNgots = await _productService.GetDoNgotsAsync();
        //    return View(new AddProductViewModel());
        //}

        //// POST: Thêm sản phẩm
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddProduct(AddProductViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _productService.AddProductAsync(model);
        //        if (result)
        //        {
        //            TempData["Success"] = "Thêm sản phẩm thành công!";
        //            return RedirectToAction("Index");
        //        }
        //        TempData["Error"] = "Không thể thêm sản phẩm. Vui lòng thử lại.";
        //    }

        //    ViewBag.Sizes = await _productService.GetSizesAsync();
        //    ViewBag.Toppings = await _productService.GetToppingsAsync();
        //    ViewBag.LuongDas = await _productService.GetLuongDasAsync();
        //    ViewBag.DoNgots = await _productService.GetDoNgotsAsync();
        //    return View(model);
        //}



        // GET: Hiển thị form thêm sản phẩm
        public async Task<IActionResult> AddProduct()
        {
            await LoadViewBagData();
            return View(new AddProductViewModel());
        }

        // POST: Thêm sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            // Kiểm tra validation server-side
            if (!ModelState.IsValid || !IsValidProduct(model))
            {
                await LoadViewBagData();
                TempData["Error"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return View(model);
            }

            try
            {
                var result = await _productService.AddProductAsync(model);
                if (result)
                {
                    TempData["Success"] = "Thêm sản phẩm thành công! Trà sữa đã sẵn sàng lên kệ! 😎";
                    return RedirectToAction("Index");
                }
                TempData["Error"] = "Không thể thêm sản phẩm. Có gì đó sai sai rồi! 😅";
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần (dùng ILogger nếu có)
                TempData["Error"] = $"Lỗi hệ thống: {ex.Message}. Liên hệ IT ngay! 🚨";
            }

            await LoadViewBagData();
            return View(model);
        }


        private async Task LoadViewBagData()
        {
            ViewBag.Sizes = await _productService.GetSizesAsync();
            ViewBag.Toppings = await _productService.GetToppingsAsync();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync();
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync();
        }

        // Kiểm tra validation bổ sung
        private bool IsValidProduct(AddProductViewModel model)
        {
            bool isValid = true;

            if (model.Gia < 1000)
            {
                ModelState.AddModelError("Gia", "Giá phải lớn hơn hoặc bằng 1.000.");
                isValid = false;
            }

            if (model.SoLuong < 1)
            {
                ModelState.AddModelError("SoLuong", "Số lượng phải lớn hơn hoặc bằng 1.");
                isValid = false;
            }

            if (model.SelectedSizes == null || !model.SelectedSizes.Any())
            {
                ModelState.AddModelError("SelectedSizes", "Phải chọn ít nhất một kích thước.");
                isValid = false;
            }

            if (model.SelectedLuongDas == null || !model.SelectedLuongDas.Any())
            {
                ModelState.AddModelError("SelectedLuongDas", "Phải chọn ít nhất một lượng đá.");
                isValid = false;
            }

            if (model.SelectedDoNgots == null || !model.SelectedDoNgots.Any())
            {
                ModelState.AddModelError("SelectedDoNgots", "Phải chọn ít nhất một độ ngọt.");
                isValid = false;
            }

            return isValid;
        }








        // GET: Hiển thị form sửa sản phẩm
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại.";
                return NotFound();
            }

            var model = new UpdateProductViewModel
            {
                ID_San_Pham = product.ID_San_Pham,
                TenSanPham = product.Ten_San_Pham,
                Gia = product.Gia,
                SoLuong = product.So_Luong,
                MoTa = product.Mo_Ta,
                TrangThai = product.Trang_Thai,
                CurrentImagePath = product.Hinh_Anh,
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(UpdateProductViewModel model)
        {
            // Loại bỏ xác thực bắt buộc cho trường Image
            if (model.Image == null && !string.IsNullOrEmpty(model.CurrentImagePath))
            {
                ModelState.Remove("Image");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Sizes = await _productService.GetSizesAsync();
                ViewBag.Toppings = await _productService.GetToppingsAsync();
                ViewBag.LuongDas = await _productService.GetLuongDasAsync();
                ViewBag.DoNgots = await _productService.GetDoNgotsAsync();
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin nhập.";
                return View(model);
            }

            try
            {
                // Gọi service để cập nhật sản phẩm
                var result = await _productService.UpdateProductAsync(model);
                if (result)
                {
                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
                TempData["Error"] = "Không thể cập nhật sản phẩm. Vui lòng thử lại.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi cập nhật sản phẩm: {ex.Message}";
            }

            ViewBag.Sizes = await _productService.GetSizesAsync();
            ViewBag.Toppings = await _productService.GetToppingsAsync();
            ViewBag.LuongDas = await _productService.GetLuongDasAsync();
            ViewBag.DoNgots = await _productService.GetDoNgotsAsync();
            return View(model);
        }
    }
}