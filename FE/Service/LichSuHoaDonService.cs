using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class LichSuHoaDonService : ILichSuHoaDonService
{
    private readonly HttpClient _httpClient;
    public LichSuHoaDonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<LichSuHoaDon>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<LichSuHoaDon>>("api/LichSuHoaDon");
    }

    public async Task<LichSuHoaDon?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<LichSuHoaDon>($"api/LichSuHoaDon/{id}");
    }

    public async Task AddAsync(LichSuHoaDon entity)
    {
        await _httpClient.PostAsJsonAsync("api/LichSuHoaDon", entity);
    }

    public async Task UpdateAsync(LichSuHoaDon entity)
    {
        await _httpClient.PutAsJsonAsync("api/LichSuHoaDon", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/LichSuHoaDon/{id}");
    }
}