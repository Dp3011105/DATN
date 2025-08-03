using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class Gio_HangService : IGio_HangService
{
    private readonly HttpClient _httpClient;
    public Gio_HangService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Gio_Hang>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Gio_Hang>>("api/Gio_Hang");
    }

    public async Task<Gio_Hang?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Gio_Hang>($"api/Gio_Hang/{id}");
    }

    public async Task AddAsync(Gio_Hang entity)
    {
        await _httpClient.PostAsJsonAsync("api/Gio_Hang", entity);
    }

    public async Task UpdateAsync(Gio_Hang entity)
    {
        await _httpClient.PutAsJsonAsync("api/Gio_Hang", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/Gio_Hang/{id}");
    }
}