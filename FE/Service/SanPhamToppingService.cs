using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class SanPhamToppingService : ISanPhamToppingService
{
    private readonly HttpClient _httpClient;
    public SanPhamToppingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<SanPhamTopping>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SanPhamTopping>>("api/SanPhamTopping");
    }

    public async Task<SanPhamTopping?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SanPhamTopping>($"api/SanPhamTopping/{id}");
    }

    public async Task AddAsync(SanPhamTopping entity)
    {
        await _httpClient.PostAsJsonAsync("api/SanPhamTopping", entity);
    }

    public async Task UpdateAsync(SanPhamTopping entity)
    {
        await _httpClient.PutAsJsonAsync("api/SanPhamTopping", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/SanPhamTopping/{id}");
    }
}