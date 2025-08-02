using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class TaiKhoanVaiTroService : ITaiKhoanVaiTroService
{
    private readonly HttpClient _httpClient;
    public TaiKhoanVaiTroService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TaiKhoanVaiTro>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<TaiKhoanVaiTro>>("api/TaiKhoanVaiTro");
    }

    public async Task<TaiKhoanVaiTro?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<TaiKhoanVaiTro>($"api/TaiKhoanVaiTro/{id}");
    }

    public async Task AddAsync(TaiKhoanVaiTro entity)
    {
        await _httpClient.PostAsJsonAsync("api/TaiKhoanVaiTro", entity);
    }

    public async Task UpdateAsync(TaiKhoanVaiTro entity)
    {
        await _httpClient.PutAsJsonAsync("api/TaiKhoanVaiTro", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/TaiKhoanVaiTro/{id}");
    }
}