using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class ToppingService : IToppingService
{
    private readonly HttpClient _httpClient;
    public ToppingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Topping>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Topping>>("api/Topping");
    }

    public async Task<Topping?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Topping>($"api/Topping/{id}");
    }

    public async Task AddAsync(Topping entity)
    {
        await _httpClient.PostAsJsonAsync("api/Topping", entity);
    }

    public async Task UpdateAsync(Topping entity)
    {
        await _httpClient.PutAsJsonAsync("api/Topping", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/Topping/{id}");
    }
}