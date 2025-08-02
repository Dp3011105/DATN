using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service.IService;
using BE.models;

public class MessageService : IMessageService
{
    private readonly HttpClient _httpClient;
    public MessageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Message>>("api/Message");
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Message>($"api/Message/{id}");
    }

    public async Task AddAsync(Message entity)
    {
        await _httpClient.PostAsJsonAsync("api/Message", entity);
    }

    public async Task UpdateAsync(Message entity)
    {
        await _httpClient.PutAsJsonAsync("api/Message", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/Message/{id}");
    }
}