using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class HoaDonVoucherService : IHoaDonVoucherService
{
    private readonly HttpClient _httpClient;
    public HoaDonVoucherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<HoaDonVoucher>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<HoaDonVoucher>>("api/HoaDonVoucher");
    }

    public async Task<HoaDonVoucher?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<HoaDonVoucher>($"api/HoaDonVoucher/{id}");
    }

    public async Task AddAsync(HoaDonVoucher entity)
    {
        await _httpClient.PostAsJsonAsync("api/HoaDonVoucher", entity);
    }

    public async Task UpdateAsync(HoaDonVoucher entity)
    {
        await _httpClient.PutAsJsonAsync("api/HoaDonVoucher", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/HoaDonVoucher/{id}");
    }
}