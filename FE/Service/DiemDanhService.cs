using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class DiemDanhService : IDiemDanhService
{
    private readonly HttpClient _httpClient;
    public DiemDanhService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<DiemDanh>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<DiemDanh>>("api/DiemDanh");
    }

    public async Task<DiemDanh?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<DiemDanh>($"api/DiemDanh/{id}");
    }

    public async Task AddAsync(DiemDanh entity)
    {
        await _httpClient.PostAsJsonAsync("api/DiemDanh", entity);
    }

    public async Task UpdateAsync(DiemDanh entity)
    {
        await _httpClient.PutAsJsonAsync("api/DiemDanh", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/DiemDanh/{id}");
    }
}