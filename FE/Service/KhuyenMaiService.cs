using FE.Models;
using FE.Service.IService;
using System.Text;
using System.Text.Json;

namespace FE.Service
{
    public class KhuyenMaiService : IKhuyenMaiService
    {
        private readonly HttpClient _httpClient;

        public KhuyenMaiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7169/api/");
        }

        public async Task<List<KhuyenMaiCrud>> GetAllPromotionsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("KhuyenMai");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Không thể lấy danh sách khuyến mãi.");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<KhuyenMaiCrud>>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khuyến mãi: {ex.Message}");
            }
        }

        public async Task<KhuyenMaiCrud> GetPromotionByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"KhuyenMai/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Không thể lấy chi tiết khuyến mãi.");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<KhuyenMaiCrud>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết khuyến mãi: {ex.Message}");
            }
        }

        public async Task<bool> AddPromotionAsync(KhuyenMaiCrud khuyenMai)
        {
            try
            {
                var json = JsonSerializer.Serialize(khuyenMai, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("KhuyenMai", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm khuyến mãi: {ex.Message}");
            }
        }

        public async Task<bool> UpdatePromotionAsync(KhuyenMaiCrud khuyenMai)
        {
            try
            {
                var json = JsonSerializer.Serialize(khuyenMai, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"KhuyenMai/{khuyenMai.ID_Khuyen_Mai}", content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Không thể sửa khuyến mãi. Status: {response.StatusCode}, Error: {errorText}");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi sửa khuyến mãi: {ex.Message}");
            }
        }


        public async Task<bool> DeletePromotionAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"KhuyenMai/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa khuyến mãi: {ex.Message}");
            }
        }
    }
}
