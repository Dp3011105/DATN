using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class VoucherService : IVoucherService
{
    private readonly HttpClient _httpClient;
    public VoucherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Voucher>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Voucher>>("api/Voucher");
    }

    public async Task<Voucher?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Voucher>($"api/Voucher/{id}");
    }

    public async Task AddAsync(Voucher entity)
    {
        await _httpClient.PostAsJsonAsync("api/Voucher", entity);
    }

    public async Task UpdateAsync(Voucher entity)
    {
        await _httpClient.PutAsJsonAsync("api/Voucher", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/Voucher/{id}");
    }
}