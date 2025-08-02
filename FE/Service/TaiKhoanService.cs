using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class TaiKhoanService : ITaiKhoanService
{
    private readonly HttpClient _httpClient;
    public TaiKhoanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TaiKhoan>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<TaiKhoan>>("api/TaiKhoan");
    }

    public async Task<TaiKhoan?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<TaiKhoan>($"api/TaiKhoan/{id}");
    }

    public async Task AddAsync(TaiKhoan entity)
    {
        await _httpClient.PostAsJsonAsync("api/TaiKhoan", entity);
    }

    public async Task UpdateAsync(TaiKhoan entity)
    {
        await _httpClient.PutAsJsonAsync("api/TaiKhoan", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/TaiKhoan/{id}");
    }
}