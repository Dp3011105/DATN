using FE.Models;
using FE.Service.IService;
using System.Text;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FE.Service
{
    public class QuanLyVaiTroService : IQuanLyVaiTroService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Để hỗ trợ nếu case không khớp hoàn toàn, nhưng dùng attribute chính
        };

        public QuanLyVaiTroService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7169/");
        }

        public async Task<List<TaiKhoanNhanVien>> GetNhanVienKhongVaiTro()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/QuanLyNhanVienVaiTro/nhan-vien-khong-vai-tro");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TaiKhoanNhanVien>>(json, _options);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên không có vai trò: {ex.Message}", ex);
            }
        }

        public async Task<List<TaiKhoanNhanVien>> GetNhanVienCoVaiTro()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/QuanLyNhanVienVaiTro/nhan-vien-co-vai-tro");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TaiKhoanNhanVien>>(json, _options);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên có vai trò: {ex.Message}", ex);
            }
        }

        public async Task<List<VaiTro>> GetAllVaiTro()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/VaiTro");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<VaiTro>>(json, _options);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách vai trò: {ex.Message}", ex);
            }
        }

        public async Task GanVaiTro(GanVaiTroRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/QuanLyNhanVienVaiTro/gan-vai-tro", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi khi gán vai trò: {ex.Message}", ex);
            }
        }

        public async Task CapNhatVaiTro(GanVaiTroRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                Console.WriteLine($"JSON gửi đi (CapNhatVaiTro): {json}");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/QuanLyNhanVienVaiTro/cap-nhat-vai-tro", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi khi cập nhật vai trò: {ex.Message}. Status Code: {ex.StatusCode}", ex);
            }
        }
    }
}
