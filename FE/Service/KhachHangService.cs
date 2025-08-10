using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.DTOs;

namespace Service
{
    public class KhachHangService : IKhachHangService
    {
        private readonly HttpClient _httpClient;

        public KhachHangService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7169/");
        }

        public async Task AddKhachHang(KhachHangDTO entity)
        {
            var response = await _httpClient.PostAsJsonAsync("api/KhachHang", entity);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteKhachHang(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/KhachHang/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<KhachHangDTO>> GetAllKhachHang()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<KhachHangDTO>>("api/KhachHang");
            return response ?? new List<KhachHangDTO>();
        }

        public async Task<KhachHangDTO> GetKhachHangById(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<KhachHangDTO>($"api/KhachHang/{id}");
            return response ?? throw new KeyNotFoundException("Customer not found.");
        }

        public async Task UpdateKhachHang(int id, KhachHangDTO entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/KhachHang/{id}", entity);
            response.EnsureSuccessStatusCode();
        }
    }
}