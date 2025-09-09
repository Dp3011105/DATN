using FE.Models;
using FE.Service.IService;

namespace FE.Service
{
    public class QLDonHangTkService : IQLDonHangTkService
    {
        private readonly HttpClient _httpClient;

        public QLDonHangTkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7169/");
        }

        public async Task<List<DonHangTK>> GetOrdersAsync(int idKhachHang)
        {
            var response = await _httpClient.GetAsync($"api/DonHangTK/{idKhachHang}");
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<List<DonHangTK>>();
            return orders?.OrderByDescending(o => o.Ngay_Tao).ToList() ?? new List<DonHangTK>();
        }

        public async Task<bool> CancelOrderAsync(DonHangTKCancelRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/DonHangTK/Cancel", request);
            return response.IsSuccessStatusCode;
        }
    }
}
