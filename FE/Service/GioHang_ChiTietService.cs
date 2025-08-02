using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class GioHang_ChiTietService : IGioHang_ChiTietService
{
    private readonly HttpClient _httpClient;
    public GioHang_ChiTietService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<GioHang_ChiTiet>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<GioHang_ChiTiet>>("api/GioHang_ChiTiet");
    }

    public async Task<GioHang_ChiTiet?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<GioHang_ChiTiet>($"api/GioHang_ChiTiet/{id}");
    }

    public async Task AddAsync(GioHang_ChiTiet entity)
    {
        await _httpClient.PostAsJsonAsync("api/GioHang_ChiTiet", entity);
    }

    public async Task UpdateAsync(GioHang_ChiTiet entity)
    {
        await _httpClient.PutAsJsonAsync("api/GioHang_ChiTiet", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/GioHang_ChiTiet/{id}");
    }
}