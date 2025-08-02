using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class KhachHangDiaChiService : IKhachHangDiaChiService
{
    private readonly HttpClient _httpClient;
    public KhachHangDiaChiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<KhachHangDiaChi>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<KhachHangDiaChi>>("api/KhachHangDiaChi");
    }

    public async Task<KhachHangDiaChi?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<KhachHangDiaChi>($"api/KhachHangDiaChi/{id}");
    }

    public async Task AddAsync(KhachHangDiaChi entity)
    {
        await _httpClient.PostAsJsonAsync("api/KhachHangDiaChi", entity);
    }

    public async Task UpdateAsync(KhachHangDiaChi entity)
    {
        await _httpClient.PutAsJsonAsync("api/KhachHangDiaChi", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/KhachHangDiaChi/{id}");
    }
}