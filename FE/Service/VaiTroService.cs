using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class VaiTroService : IVaiTroService
{
    private readonly HttpClient _httpClient;
    public VaiTroService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7169/");
    }

    public async Task AddAsync(VaiTro entity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/VaiTro", entity);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to add role: {response.ReasonPhrase}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var response =await _httpClient.DeleteAsync($"api/VaiTro/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to delete role with ID {id}: {response.ReasonPhrase}");
        }
    }

    public async Task<IEnumerable<VaiTro>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<VaiTro>>("api/VaiTro");
    }

    public async Task<VaiTro> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<VaiTro>($"api/VaiTro/{id}");
    }

    public async Task UpdateAsync(int id, VaiTro entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/VaiTro/{id}", entity);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to update role with ID {id}: {response.ReasonPhrase}");
        }
    }
}