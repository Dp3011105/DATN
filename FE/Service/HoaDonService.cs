
using BE.DTOs;
using BE.models;
using Service.IService;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class HoaDonService : IHoaDonService
{
    private readonly HttpClient _httpClient;
    public HoaDonService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7169/");
    }

    public async Task AddAsync(HoaDon entity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/HoaDon", entity);
        response.EnsureSuccessStatusCode();
    }


    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/HoaDon/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<HoaDon>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<HoaDon>>("api/HoaDon");
    }

    public async Task<HoaDon> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<HoaDon>($"api/HoaDon/{id}");
    }

    public async Task UpdateAsync(int id, HoaDon entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/HoaDon/{id}", entity);
        response.EnsureSuccessStatusCode();
    }
}