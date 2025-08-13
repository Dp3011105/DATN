using System.Net.Http;
using System.Net.Http.Json;
using BE.models;
using Service.IService;

namespace Service.Services // hoặc namespace của bạn cho layer Service
{
    public class HoaDonService : IHoaDonService
    {
        private readonly HttpClient _httpClient;

        public HoaDonService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7169/");
        }

        public async Task<IEnumerable<HoaDon>> GetAllAsync()
            => await _httpClient.GetFromJsonAsync<IEnumerable<HoaDon>>("api/HoaDon");

        public async Task<HoaDon> GetByIdAsync(int id)
            => await _httpClient.GetFromJsonAsync<HoaDon>($"api/HoaDon/{id}");

        public async Task AddAsync(HoaDon entity)
        {
            var res = await _httpClient.PostAsJsonAsync("api/HoaDon", entity);
            res.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, HoaDon entity)
        {
            var res = await _httpClient.PutAsJsonAsync($"api/HoaDon/{id}", entity);
            res.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var res = await _httpClient.DeleteAsync($"api/HoaDon/{id}");
            res.EnsureSuccessStatusCode();
        }
    }
}
