using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class SanPhamService : ISanPhamService
{
    private readonly HttpClient _httpClient;
    public SanPhamService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<SanPham>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SanPham>>("api/SanPham");
    }

    public async Task<SanPham?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SanPham>($"api/SanPham/{id}");
    }

    public async Task AddAsync(SanPham entity)
    {
        await _httpClient.PostAsJsonAsync("api/SanPham", entity);
    }

    public async Task UpdateAsync(SanPham entity)
    {
        await _httpClient.PutAsJsonAsync("api/SanPham", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/SanPham/{id}");
    }
}