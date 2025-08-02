using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class GioHangChiTiet_ToppingService : IGioHangChiTiet_ToppingService
{
    private readonly HttpClient _httpClient;
    public GioHangChiTiet_ToppingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<GioHangChiTiet_Topping>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<GioHangChiTiet_Topping>>("api/GioHangChiTiet_Topping");
    }

    public async Task<GioHangChiTiet_Topping?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<GioHangChiTiet_Topping>($"api/GioHangChiTiet_Topping/{id}");
    }

    public async Task AddAsync(GioHangChiTiet_Topping entity)
    {
        await _httpClient.PostAsJsonAsync("api/GioHangChiTiet_Topping", entity);
    }

    public async Task UpdateAsync(GioHangChiTiet_Topping entity)
    {
        await _httpClient.PutAsJsonAsync("api/GioHangChiTiet_Topping", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/GioHangChiTiet_Topping/{id}");
    }
}