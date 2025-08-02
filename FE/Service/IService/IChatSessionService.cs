using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IChatSessionService
    {
        Task<IEnumerable<ChatSession>> GetAllAsync();
        Task<ChatSession?> GetByIdAsync(int id);
        Task AddAsync(ChatSession entity);
        Task UpdateAsync(ChatSession entity);
        Task DeleteAsync(int id);
    }
}