using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class SizeService : ISizeService
{
    private readonly HttpClient _httpClient;
    public SizeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Size>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Size>>("api/Size");
    }

    public async Task<Size?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Size>($"api/Size/{id}");
    }

    public async Task AddAsync(Size entity)
    {
        await _httpClient.PostAsJsonAsync("api/Size", entity);
    }

    public async Task UpdateAsync(Size entity)
    {
        await _httpClient.PutAsJsonAsync("api/Size", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/Size/{id}");
    }
}