using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class LuongDaService : ILuongDaService
{
    private readonly HttpClient _httpClient;
    public LuongDaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<LuongDa>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<LuongDa>>("api/LuongDa");
    }

    public async Task<LuongDa?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<LuongDa>($"api/LuongDa/{id}");
    }

    public async Task AddAsync(LuongDa entity)
    {
        await _httpClient.PostAsJsonAsync("api/LuongDa", entity);
    }

    public async Task UpdateAsync(LuongDa entity)
    {
        await _httpClient.PutAsJsonAsync("api/LuongDa", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/LuongDa/{id}");
    }
}