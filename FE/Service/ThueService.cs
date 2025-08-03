using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class ThueService : IThueService
{
    private readonly HttpClient _httpClient;
    public ThueService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Thue>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Thue>>("api/Thue");
    }

    public async Task<Thue?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Thue>($"api/Thue/{id}");
    }

    public async Task AddAsync(Thue entity)
    {
        await _httpClient.PostAsJsonAsync("api/Thue", entity);
    }

    public async Task UpdateAsync(Thue entity)
    {
        await _httpClient.PutAsJsonAsync("api/Thue", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/Thue/{id}");
    }
}