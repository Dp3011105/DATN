using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class KhachHangService : IKhachHangService
{
    private readonly HttpClient _httpClient;
    public KhachHangService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<KhachHang>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<KhachHang>>("api/KhachHang");
    }

    public async Task<KhachHang?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<KhachHang>($"api/KhachHang/{id}");
    }

    public async Task AddAsync(KhachHang entity)
    {
        await _httpClient.PostAsJsonAsync("api/KhachHang", entity);
    }

    public async Task UpdateAsync(KhachHang entity)
    {
        await _httpClient.PutAsJsonAsync("api/KhachHang", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/KhachHang/{id}");
    }
}