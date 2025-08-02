using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class SanPhamLuongDaService : ISanPhamLuongDaService
{
    private readonly HttpClient _httpClient;
    public SanPhamLuongDaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<SanPhamLuongDa>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SanPhamLuongDa>>("api/SanPhamLuongDa");
    }

    public async Task<SanPhamLuongDa?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SanPhamLuongDa>($"api/SanPhamLuongDa/{id}");
    }

    public async Task AddAsync(SanPhamLuongDa entity)
    {
        await _httpClient.PostAsJsonAsync("api/SanPhamLuongDa", entity);
    }

    public async Task UpdateAsync(SanPhamLuongDa entity)
    {
        await _httpClient.PutAsJsonAsync("api/SanPhamLuongDa", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/SanPhamLuongDa/{id}");
    }
}