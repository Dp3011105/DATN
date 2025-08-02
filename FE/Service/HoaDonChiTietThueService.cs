using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class HoaDonChiTietThueService : IHoaDonChiTietThueService
{
    private readonly HttpClient _httpClient;
    public HoaDonChiTietThueService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<HoaDonChiTietThue>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<HoaDonChiTietThue>>("api/HoaDonChiTietThue");
    }

    public async Task<HoaDonChiTietThue?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<HoaDonChiTietThue>($"api/HoaDonChiTietThue/{id}");
    }

    public async Task AddAsync(HoaDonChiTietThue entity)
    {
        await _httpClient.PostAsJsonAsync("api/HoaDonChiTietThue", entity);
    }

    public async Task UpdateAsync(HoaDonChiTietThue entity)
    {
        await _httpClient.PutAsJsonAsync("api/HoaDonChiTietThue", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/HoaDonChiTietThue/{id}");
    }
}