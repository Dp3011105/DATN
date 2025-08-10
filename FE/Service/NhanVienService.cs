using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class NhanVienService : INhanVienService
{
    private readonly HttpClient _httpClient;
    public NhanVienService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7169/");
    }

    public async Task AddAsync(NhanVien entity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/NhanVien", entity);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/NhanVien/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<NhanVien>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<NhanVien>>("api/NhanVien");
    }

    public async Task<NhanVien> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<NhanVien>($"api/NhanVien/{id}");
    }

    public async Task UpdateAsync(int id, NhanVien entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/NhanVien/{id}", entity);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Update failed: {response.StatusCode} - {error}");
        }
    }
}