using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class KhachHangVoucherService : IKhachHangVoucherService
{
    private readonly HttpClient _httpClient;
    public KhachHangVoucherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<KhachHangVoucher>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<KhachHangVoucher>>("api/KhachHangVoucher");
    }

    public async Task<KhachHangVoucher?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<KhachHangVoucher>($"api/KhachHangVoucher/{id}");
    }

    public async Task AddAsync(KhachHangVoucher entity)
    {
        await _httpClient.PostAsJsonAsync("api/KhachHangVoucher", entity);
    }

    public async Task UpdateAsync(KhachHangVoucher entity)
    {
        await _httpClient.PutAsJsonAsync("api/KhachHangVoucher", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/KhachHangVoucher/{id}");
    }
}