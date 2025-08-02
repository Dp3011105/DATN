using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class HoaDonChiTietService : IHoaDonChiTietService
{
    private readonly HttpClient _httpClient;
    public HoaDonChiTietService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<HoaDonChiTiet>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<HoaDonChiTiet>>("api/HoaDonChiTiet");
    }

    public async Task<HoaDonChiTiet?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<HoaDonChiTiet>($"api/HoaDonChiTiet/{id}");
    }

    public async Task AddAsync(HoaDonChiTiet entity)
    {
        await _httpClient.PostAsJsonAsync("api/HoaDonChiTiet", entity);
    }

    public async Task UpdateAsync(HoaDonChiTiet entity)
    {
        await _httpClient.PutAsJsonAsync("api/HoaDonChiTiet", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/HoaDonChiTiet/{id}");
    }
}