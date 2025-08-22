using FE.Models;
using FE.Service.IService;

namespace FE.Service
{// dùng cho chức năng khuyến mãi, quản lý các chương trình khuyến mãi cho sản phẩm
    public class PromotionService : IPromotionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PromotionService> _logger;

        public PromotionService(HttpClient httpClient, ILogger<PromotionService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Promotion>> GetActivePromotionsAsync()
        {
            _logger.LogDebug("Bắt đầu gọi API lấy danh sách khuyến mãi: https://localhost:7169/api/KhuyenMai");
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7169/api/KhuyenMai");
                _logger.LogDebug($"Phản hồi từ API KhuyenMai: StatusCode = {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API KhuyenMai trả về lỗi: StatusCode = {response.StatusCode}, Content = {errorContent}");
                    return new List<Promotion>();
                }

                var promotions = await response.Content.ReadFromJsonAsync<List<Promotion>>();
                _logger.LogDebug($"Dữ liệu thô từ API KhuyenMai: {System.Text.Json.JsonSerializer.Serialize(promotions)}");
                _logger.LogDebug($"Số lượng khuyến mãi nhận được: {(promotions?.Count ?? 0)}");

                var activePromotions = promotions?.Where(p => p.Status).ToList() ?? new List<Promotion>();
                _logger.LogDebug($"Số lượng khuyến mãi có trạng thái true: {activePromotions.Count}");
                return activePromotions;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API KhuyenMai: {Message}", ex.Message);
                return new List<Promotion>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi lấy danh sách khuyến mãi: {Message}", ex.Message);
                return new List<Promotion>();
            }
        }

        public async Task<List<Product>> GetProductsByPromotionAsync()
        {
            _logger.LogDebug("Bắt đầu gọi API lấy danh sách sản phẩm: https://localhost:7169/api/KhuyenMaiSanPham");
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7169/api/KhuyenMaiSanPham");
                _logger.LogDebug($"Phản hồi từ API KhuyenMaiSanPham: StatusCode = {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API KhuyenMaiSanPham trả về lỗi: StatusCode = {response.StatusCode}, Content = {errorContent}");
                    return new List<Product>();
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogDebug($"Dữ liệu thô từ API KhuyenMaiSanPham: {jsonString}");

                try
                {
                    var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                    _logger.LogDebug($"Số lượng sản phẩm nhận được: {(products?.Count ?? 0)}");

                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            _logger.LogDebug($"Sản phẩm: ID = {product.ID_Product}, Name = {product.ProductName}, sanPhamKhuyenMai = {(product.ProductPromotion == null ? "null" : System.Text.Json.JsonSerializer.Serialize(product.ProductPromotion))}");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Dữ liệu sản phẩm deserialize thành null");
                    }

                    // Chỉ lấy sản phẩm có sanPhamKhuyenMai == null
                    var filteredProducts = products?.Where(p => p.ProductPromotion == null).ToList() ?? new List<Product>();
                    _logger.LogDebug($"Số lượng sản phẩm sau khi lọc (sanPhamKhuyenMai == null): {filteredProducts.Count}");

                    // Debug: Trả về toàn bộ sản phẩm để kiểm tra
                    return products ?? new List<Product>();
                    // return filteredProducts;
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogError(ex, "Lỗi deserialize JSON sản phẩm: {Message}", ex.Message);
                    _logger.LogError($"JSON gây lỗi: {jsonString}");
                    return new List<Product>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API KhuyenMaiSanPham: {Message}", ex.Message);
                return new List<Product>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi lấy danh sách sản phẩm: {Message}", ex.Message);
                return new List<Product>();
            }
        }

        public async Task<bool> AssignPromotionAsync(AssignPromotionRequest request)
        {
            _logger.LogDebug("Bắt đầu gọi API gán khuyến mãi: https://localhost:7169/api/KhuyenMaiSanPham/assign");
            _logger.LogDebug($"Dữ liệu gửi đi: {System.Text.Json.JsonSerializer.Serialize(request)}");
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7169/api/KhuyenMaiSanPham/assign", request);
                _logger.LogDebug($"Phản hồi từ API AssignPromotion: StatusCode = {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API AssignPromotion trả về lỗi: StatusCode = {response.StatusCode}, Content = {errorContent}");
                }
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API AssignPromotion: {Message}", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi gán khuyến mãi: {Message}", ex.Message);
                return false;
            }
        }
    }
}
