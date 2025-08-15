using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class TaiKhoanService : ITaiKhoanService
{
    private readonly HttpClient _httpClient;
    public TaiKhoanService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7169/");
    }

    public async Task AddAsync(TaiKhoan entity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/TaiKhoan", entity);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error adding TaiKhoan: {response.ReasonPhrase}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/TaiKhoan/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error deleting TaiKhoan with ID {id}: {response.ReasonPhrase}");
        }
    }

    public async Task<IEnumerable<TaiKhoan>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<TaiKhoan>>("api/TaiKhoan");
    }

    public async Task<TaiKhoan> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<TaiKhoan>($"api/TaiKhoan/{id}");
    }

    public async Task UpdateAsync(int id, TaiKhoan entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/TaiKhoan/{id}", entity);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error updating TaiKhoan with ID {id}: {response.ReasonPhrase}");
        }
    }
}