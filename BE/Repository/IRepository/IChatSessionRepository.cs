using BE.models;

namespace Repository.IRepository
{
    public interface IChatSessionRepository
    {
        Task<IEnumerable<ChatSession>> GetAllAsync();
        Task<ChatSession?> GetByIdAsync(int id);
        Task AddAsync(ChatSession entity);
        Task UpdateAsync(ChatSession entity);
        Task DeleteAsync(int id);
    }
}