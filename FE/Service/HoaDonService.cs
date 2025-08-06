using BE.models;

public class HoaDonService : IHoaDonService
{
    private readonly HttpClient _httpClient;
    public HoaDonService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<HoaDon?> GetByIdAsync(int id)
        => await _httpClient.GetFromJsonAsync<HoaDon>($"api/HoaDon/{id}");

    public async Task AddAsync(HoaDon entity)
        => await _httpClient.PostAsJsonAsync("api/HoaDon", entity);

    public async Task UpdateAsync(HoaDon entity)
        => await _httpClient.PutAsJsonAsync("api/HoaDon", entity);

    public async Task DeleteAsync(int id)
        => await _httpClient.DeleteAsync($"api/HoaDon/{id}");
    public async Task<IEnumerable<HoaDonDTO>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<HoaDonDTO>>("api/HoaDon");
    }
}
