using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class ChatSessionNhanVienService : IChatSessionNhanVienService
{
    private readonly HttpClient _httpClient;
    public ChatSessionNhanVienService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ChatSessionNhanVien>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ChatSessionNhanVien>>("api/ChatSessionNhanVien");
    }

    public async Task<ChatSessionNhanVien?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ChatSessionNhanVien>($"api/ChatSessionNhanVien/{id}");
    }

    public async Task AddAsync(ChatSessionNhanVien entity)
    {
        await _httpClient.PostAsJsonAsync("api/ChatSessionNhanVien", entity);
    }

    public async Task UpdateAsync(ChatSessionNhanVien entity)
    {
        await _httpClient.PutAsJsonAsync("api/ChatSessionNhanVien", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/ChatSessionNhanVien/{id}");
    }
}