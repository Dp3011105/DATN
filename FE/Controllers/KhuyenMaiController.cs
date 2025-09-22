using FE.Filters;
using FE.Models;
using FE.Service;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
namespace FE.Controllers// controler dùng để thực hiện các chức năng liên quan đến khuyến mãi, crud quản lý khuyến mãi
{
    [RoleAuthorize(2)]// Phương thức này đươc để trong thư mục Filters nhé ae

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
        public async Task<IActionResult> Index(string searchTerm = "", string statusFilter = "", int page = 1, int pageSize = 10)
        {
            try
            {
                var khuyenMais = await _khuyenMaiService.GetAllPromotionsAsync();
                var currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

                // Lọc
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    khuyenMais = khuyenMais.Where(k => k.Ten_Khuyen_Mai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    if (statusFilter == "true")
                        khuyenMais = khuyenMais.Where(k => k.Trang_Thai && TimeZoneInfo.ConvertTimeFromUtc(k.Ngay_Ket_Thuc, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")) > currentDate).ToList();
                    else if (statusFilter == "false")
                        khuyenMais = khuyenMais.Where(k => !k.Trang_Thai && TimeZoneInfo.ConvertTimeFromUtc(k.Ngay_Ket_Thuc, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")) > currentDate).ToList();
                    else if (statusFilter == "expired")
                        khuyenMais = khuyenMais.Where(k => TimeZoneInfo.ConvertTimeFromUtc(k.Ngay_Ket_Thuc, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")) <= currentDate).ToList();
                }

                // Thống kê
                ViewBag.TotalPromotions = khuyenMais.Count;
                ViewBag.ActivePromotions = khuyenMais.Count(k => k.Trang_Thai && TimeZoneInfo.ConvertTimeFromUtc(k.Ngay_Ket_Thuc, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")) > currentDate);
                ViewBag.StoppedPromotions = khuyenMais.Count(k => !k.Trang_Thai && TimeZoneInfo.ConvertTimeFromUtc(k.Ngay_Ket_Thuc, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")) > currentDate);
                ViewBag.ExpiredPromotions = khuyenMais.Count(k => TimeZoneInfo.ConvertTimeFromUtc(k.Ngay_Ket_Thuc, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")) <= currentDate);

                // Phân trang
                var totalItems = khuyenMais.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                var paginatedItems = khuyenMais.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = page;
                ViewBag.SearchTerm = searchTerm;
                ViewBag.StatusFilter = statusFilter;

                return View(paginatedItems);
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
