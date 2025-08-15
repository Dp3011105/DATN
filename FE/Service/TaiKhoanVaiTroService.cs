using System.Net.Http;
using System.Net.Http.Json;
using Service.IService;
using BE.models;
using BE.DTOs;

public class TaiKhoanVaiTroService : ITaiKhoanVaiTroService
{
    private readonly HttpClient _httpClient;
    public TaiKhoanVaiTroService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7169/");
    }

    public async Task CreateTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro)
    {
        await _httpClient.PostAsJsonAsync("api/TaiKhoanVaiTro", taiKhoanVaiTro);
    }

    public async Task DeleteTaiKhoanVaiTroAsync(int idTaiKhoan, int idVaiTro)
    {
        await _httpClient.DeleteAsync($"api/TaiKhoanVaiTro/{idTaiKhoan}/{idVaiTro}");
    }

    public async Task<IEnumerable<TaiKhoan>> GetAllTaiKhoanNhanVienAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<TaiKhoan>>("api/TaiKhoan");
    }

    public async Task<IEnumerable<TaiKhoanVaiTroDTO>> GetAllTaiKhoanVaiTroAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<TaiKhoanVaiTroDTO>>("api/TaiKhoanVaiTro");
    }

    public async Task<IEnumerable<VaiTro>> GetAllVaiTroAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<VaiTro>>("api/VaiTro");
    }

    public async Task<TaiKhoanVaiTroDTO> GetTaiKhoanVaiTroByIdAsync(int idTaiKhoan, int idVaiTro)
    {
        return await _httpClient.GetFromJsonAsync<TaiKhoanVaiTroDTO>($"api/TaiKhoanVaiTro/{idTaiKhoan}/{idVaiTro}");
    }

    public async Task UpdateTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro)
    {
        await _httpClient.PutAsJsonAsync($"api/TaiKhoanVaiTro/{taiKhoanVaiTro.ID_Tai_Khoan}/{taiKhoanVaiTro.ID_Vai_Tro}", taiKhoanVaiTro);
    }
}
