using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class HinhThucThanhToanService : IHinhThucThanhToanService
{
    private readonly HttpClient _httpClient;
    public HinhThucThanhToanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<HinhThucThanhToan>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<HinhThucThanhToan>>("api/HinhThucThanhToan");
    }

    public async Task<HinhThucThanhToan?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<HinhThucThanhToan>($"api/HinhThucThanhToan/{id}");
    }

    public async Task AddAsync(HinhThucThanhToan entity)
    {
        await _httpClient.PostAsJsonAsync("api/HinhThucThanhToan", entity);
    }

    public async Task UpdateAsync(HinhThucThanhToan entity)
    {
        await _httpClient.PutAsJsonAsync("api/HinhThucThanhToan", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/HinhThucThanhToan/{id}");
    }
}