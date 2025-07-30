using FE.Models;
using FE.Service.IService;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FE.Services
{
    public class ProductDetailsService : IProductDetailsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7169/api/";

        public ProductDetailsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // DoNgot
        public async Task<List<DoNgot>> GetDoNgotsAsync()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "DoNgot");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoNgot>>(content);
        }

        public async Task<DoNgot> GetDoNgotByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(_baseUrl + $"DoNgot/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DoNgot>(content);
        }

        public async Task<bool> AddDoNgotAsync(DoNgot doNgot)
        {
            var json = JsonConvert.SerializeObject(doNgot);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl + "DoNgot", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoNgotAsync(DoNgot doNgot)
        {
            var json = JsonConvert.SerializeObject(doNgot);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(_baseUrl + $"DoNgot/{doNgot.ID_DoNgot}", content);
            return response.IsSuccessStatusCode;
        }

        // Size
        public async Task<List<Size>> GetSizesAsync()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "Size");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Size>>(content);
        }

        public async Task<Size> GetSizeByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(_baseUrl + $"Size/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Size>(content);
        }

        public async Task<bool> AddSizeAsync(Size size)
        {
            var json = JsonConvert.SerializeObject(size);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl + "Size", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSizeAsync(Size size)
        {
            var json = JsonConvert.SerializeObject(size);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(_baseUrl + $"Size/{size.ID_Size}", content);
            return response.IsSuccessStatusCode;
        }

        // Topping
        public async Task<List<Topping>> GetToppingsAsync()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "Topping");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Topping>>(content);
        }

        public async Task<Topping> GetToppingByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(_baseUrl + $"Topping/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Topping>(content);
        }

        public async Task<bool> AddToppingAsync(Topping topping)
        {
            var json = JsonConvert.SerializeObject(topping);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl + "Topping", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateToppingAsync(Topping topping)
        {
            var json = JsonConvert.SerializeObject(topping);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(_baseUrl + $"Topping/{topping.ID_Topping}", content);
            return response.IsSuccessStatusCode;
        }

        // LuongDa
        public async Task<List<LuongDa>> GetLuongDasAsync()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "LuongDa");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<LuongDa>>(content);
        }

        public async Task<LuongDa> GetLuongDaByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(_baseUrl + $"LuongDa/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LuongDa>(content);
        }

        public async Task<bool> AddLuongDaAsync(LuongDa luongDa)
        {
            var json = JsonConvert.SerializeObject(luongDa);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl + "LuongDa", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLuongDaAsync(LuongDa luongDa)
        {
            var json = JsonConvert.SerializeObject(luongDa);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(_baseUrl + $"LuongDa/{luongDa.ID_LuongDa}", content);
            return response.IsSuccessStatusCode;
        }
    }
}