using FE.Service.IService;
using FE.Models;
using System.Text;
using System.Text.Json;

namespace FE.Service
{
    public class ProductDetailsService : IProductDetailsService
    {
        private readonly HttpClient _httpClient;

        public ProductDetailsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // DoNgot
        private const string DoNgotApiBaseUrl = "https://localhost:7169/api/DoNgot";
        public async Task<List<DoNgot>> GetAllDoNgotsAsync()
        {
            var response = await _httpClient.GetAsync(DoNgotApiBaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DoNgot>>(content);
        }

        public async Task<DoNgot> GetDoNgotByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{DoNgotApiBaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DoNgot>(content);
        }

        public async Task CreateDoNgotAsync(DoNgot doNgot)
        {
            var content = new StringContent(JsonSerializer.Serialize(doNgot), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(DoNgotApiBaseUrl, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateDoNgotAsync(DoNgot doNgot)
        {
            var content = new StringContent(JsonSerializer.Serialize(doNgot), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{DoNgotApiBaseUrl}/{doNgot.ID_DoNgot}", content);
            response.EnsureSuccessStatusCode();
        }

        // Size
        private const string SizeApiBaseUrl = "https://localhost:7169/api/Size";
        public async Task<List<Size>> GetAllSizesAsync()
        {
            var response = await _httpClient.GetAsync(SizeApiBaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Size>>(content);
        }

        public async Task<Size> GetSizeByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{SizeApiBaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Size>(content);
        }

        public async Task CreateSizeAsync(Size size)
        {
            var content = new StringContent(JsonSerializer.Serialize(size), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(SizeApiBaseUrl, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateSizeAsync(Size size)
        {
            var content = new StringContent(JsonSerializer.Serialize(size), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{SizeApiBaseUrl}/{size.ID_Size}", content);
            response.EnsureSuccessStatusCode();
        }

        // Topping
        private const string ToppingApiBaseUrl = "https://localhost:7169/api/Topping";
        public async Task<List<Topping>> GetAllToppingsAsync()
        {
            var response = await _httpClient.GetAsync(ToppingApiBaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Topping>>(content);
        }

        public async Task<Topping> GetToppingByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{ToppingApiBaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Topping>(content);
        }

        public async Task CreateToppingAsync(Topping topping)
        {
            var content = new StringContent(JsonSerializer.Serialize(topping), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ToppingApiBaseUrl, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateToppingAsync(Topping topping)
        {
            var content = new StringContent(JsonSerializer.Serialize(topping), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{ToppingApiBaseUrl}/{topping.ID_Topping}", content);
            response.EnsureSuccessStatusCode();
        }

        // LuongDa
        // LuongDa
        private const string LuongDaApiBaseUrl = "https://localhost:7169/api/LuongDa";
        public async Task<List<LuongDa>> GetAllLuongDasAsync()
        {
            var response = await _httpClient.GetAsync(LuongDaApiBaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<LuongDa>>(content);
        }

        public async Task<LuongDa> GetLuongDaByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{LuongDaApiBaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LuongDa>(content);
        }

        public async Task CreateLuongDaAsync(LuongDa luongDa)
        {
            var content = new StringContent(JsonSerializer.Serialize(luongDa), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(LuongDaApiBaseUrl, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateLuongDaAsync(LuongDa luongDa)
        {
            var content = new StringContent(JsonSerializer.Serialize(luongDa), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{LuongDaApiBaseUrl}/{luongDa.ID_LuongDa}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
