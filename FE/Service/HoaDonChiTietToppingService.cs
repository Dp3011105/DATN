using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class HoaDonChiTietToppingService : IHoaDonChiTietToppingService
{
    private readonly HttpClient _httpClient;
    public HoaDonChiTietToppingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<HoaDonChiTietTopping>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<HoaDonChiTietTopping>>("api/HoaDonChiTietTopping");
    }

    public async Task<HoaDonChiTietTopping?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<HoaDonChiTietTopping>($"api/HoaDonChiTietTopping/{id}");
    }

    public async Task AddAsync(HoaDonChiTietTopping entity)
    {
        await _httpClient.PostAsJsonAsync("api/HoaDonChiTietTopping", entity);
    }

    public async Task UpdateAsync(HoaDonChiTietTopping entity)
    {
        await _httpClient.PutAsJsonAsync("api/HoaDonChiTietTopping", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/HoaDonChiTietTopping/{id}");
    }
}