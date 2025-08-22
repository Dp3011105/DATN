using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{// controller này dùng để thực hiện các chức năng liên quan đến khuyến mãi sản phẩm,thêm khuyến mãi cho sản phẩm , hủy áp dụng khuyến mãi cho sản phẩm
    public class KhuyenMaiSanPhamController : Controller
    {
        private readonly IPromotionService _promotionService;
        private readonly ILogger<KhuyenMaiSanPhamController> _logger;

        public KhuyenMaiSanPhamController(IPromotionService promotionService, ILogger<KhuyenMaiSanPhamController> logger)
        {
            _promotionService = promotionService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogDebug("Bắt đầu action Index");
            try
            {
                var promotions = await _promotionService.GetActivePromotionsAsync();

                _logger.LogDebug($"Số lượng khuyến mãi truyền vào View: {promotions.Count}");
                _logger.LogDebug($"Dữ liệu khuyến mãi truyền vào View: {System.Text.Json.JsonSerializer.Serialize(promotions)}");

                ViewBag.Promotions = promotions;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong action Index: {Message}", ex.Message);
                ViewBag.Promotions = new List<Promotion>();
                ViewBag.Products = new List<Product>();
                TempData["Error"] = "Có lỗi xảy ra khi tải dữ liệu.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignPromotion(int selectedPromotionId, List<int> selectedProductIds, int discountPercentage)
        {
            _logger.LogDebug($"Nhận yêu cầu AssignPromotion: selectedPromotionId={selectedPromotionId}, selectedProductIds={string.Join(",", selectedProductIds ?? new List<int>())}), discountPercentage={discountPercentage}");

            if (selectedPromotionId <= 0 || selectedProductIds == null || !selectedProductIds.Any() || discountPercentage < 1 || discountPercentage > 90)
            {
                _logger.LogWarning("Dữ liệu không hợp lệ: selectedPromotionId={selectedPromotionId}, selectedProductIds={selectedProductIds}, discountPercentage={discountPercentage}",
                    selectedPromotionId, selectedProductIds?.Count ?? 0, discountPercentage);
                TempData["Error"] = "Vui lòng chọn khuyến mãi, sản phẩm và nhập phần trăm giảm giá hợp lệ (1-90%).";
                return RedirectToAction("Index");
            }

            var request = new AssignPromotionRequest
            {
                ID_Promotion = selectedPromotionId,
                ID_Products = selectedProductIds,
                DiscountPercentage = discountPercentage
            };

            var success = await _promotionService.AssignPromotionAsync(request);
            if (success)
            {
                _logger.LogInformation("Áp dụng khuyến mãi thành công: {request}", System.Text.Json.JsonSerializer.Serialize(request));
                TempData["Success"] = "Áp dụng khuyến mãi thành công!";
            }
            else
            {
                _logger.LogError("Lỗi khi áp dụng khuyến mãi: {request}", System.Text.Json.JsonSerializer.Serialize(request));
                TempData["Error"] = "Có lỗi xảy ra khi áp dụng khuyến mãi.";
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> CancelPromotion()
        {
            _logger.LogDebug("Bắt đầu action CancelPromotion");
            try
            {
                var promotions = await _promotionService.GetActivePromotionsAsync();
                ViewBag.Promotions = promotions;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong action CancelPromotion: {Message}", ex.Message);
                ViewBag.Promotions = new List<Promotion>();
                TempData["Error"] = "Có lỗi xảy ra khi tải dữ liệu.";
                return View();
            }
        }



    }
}
