using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class SanPhamDoNgotService : ISanPhamDoNgotService
{
    private readonly HttpClient _httpClient;
    public SanPhamDoNgotService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<SanPhamDoNgot>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SanPhamDoNgot>>("api/SanPhamDoNgot");
    }

    public async Task<SanPhamDoNgot?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SanPhamDoNgot>($"api/SanPhamDoNgot/{id}");
    }

    public async Task AddAsync(SanPhamDoNgot entity)
    {
        await _httpClient.PostAsJsonAsync("api/SanPhamDoNgot", entity);
    }

    public async Task UpdateAsync(SanPhamDoNgot entity)
    {
        await _httpClient.PutAsJsonAsync("api/SanPhamDoNgot", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/SanPhamDoNgot/{id}");
    }
}