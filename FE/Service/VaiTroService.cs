using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class VaiTroService : IVaiTroService
{
    private readonly HttpClient _httpClient;
    public VaiTroService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<VaiTro>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<VaiTro>>("api/VaiTro");
    }

    public async Task<VaiTro?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<VaiTro>($"api/VaiTro/{id}");
    }

    public async Task AddAsync(VaiTro entity)
    {
        await _httpClient.PostAsJsonAsync("api/VaiTro", entity);
    }

    public async Task UpdateAsync(VaiTro entity)
    {
        await _httpClient.PutAsJsonAsync("api/VaiTro", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/VaiTro/{id}");
    }
}