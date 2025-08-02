using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class DiaChiService : IDiaChiService
{
    private readonly HttpClient _httpClient;
    public DiaChiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<DiaChi>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<DiaChi>>("api/DiaChi");
    }

    public async Task<DiaChi?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<DiaChi>($"api/DiaChi/{id}");
    }

    public async Task AddAsync(DiaChi entity)
    {
        await _httpClient.PostAsJsonAsync("api/DiaChi", entity);
    }

    public async Task UpdateAsync(DiaChi entity)
    {
        await _httpClient.PutAsJsonAsync("api/DiaChi", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/DiaChi/{id}");
    }
}