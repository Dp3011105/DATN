using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class ChatSessionService : IChatSessionService
{
    private readonly HttpClient _httpClient;
    public ChatSessionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ChatSession>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ChatSession>>("api/ChatSession");
    }

    public async Task<ChatSession?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ChatSession>($"api/ChatSession/{id}");
    }

    public async Task AddAsync(ChatSession entity)
    {
        await _httpClient.PostAsJsonAsync("api/ChatSession", entity);
    }

    public async Task UpdateAsync(ChatSession entity)
    {
        await _httpClient.PutAsJsonAsync("api/ChatSession", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/ChatSession/{id}");
    }
}