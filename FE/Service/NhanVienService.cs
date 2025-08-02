using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class NhanVienService : INhanVienService
{
    private readonly HttpClient _httpClient;
    public NhanVienService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<NhanVien>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<NhanVien>>("api/NhanVien");
    }

    public async Task<NhanVien?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<NhanVien>($"api/NhanVien/{id}");
    }

    public async Task AddAsync(NhanVien entity)
    {
        await _httpClient.PostAsJsonAsync("api/NhanVien", entity);
    }

    public async Task UpdateAsync(NhanVien entity)
    {
        await _httpClient.PutAsJsonAsync("api/NhanVien", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/NhanVien/{id}");
    }
}