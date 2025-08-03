using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class SanPhamSizeService : ISanPhamSizeService
{
    private readonly HttpClient _httpClient;
    public SanPhamSizeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<SanPhamSize>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SanPhamSize>>("api/SanPhamSize");
    }

    public async Task<SanPhamSize?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SanPhamSize>($"api/SanPhamSize/{id}");
    }

    public async Task AddAsync(SanPhamSize entity)
    {
        await _httpClient.PostAsJsonAsync("api/SanPhamSize", entity);
    }

    public async Task UpdateAsync(SanPhamSize entity)
    {
        await _httpClient.PutAsJsonAsync("api/SanPhamSize", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/SanPhamSize/{id}");
    }
}