using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class DoNgotService : IDoNgotService
{
    private readonly HttpClient _httpClient;
    public DoNgotService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<DoNgot>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<DoNgot>>("api/DoNgot");
    }

    public async Task<DoNgot?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<DoNgot>($"api/DoNgot/{id}");
    }

    public async Task AddAsync(DoNgot entity)
    {
        await _httpClient.PostAsJsonAsync("api/DoNgot", entity);
    }

    public async Task UpdateAsync(DoNgot entity)
    {
        await _httpClient.PutAsJsonAsync("api/DoNgot", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/DoNgot/{id}");
    }
}