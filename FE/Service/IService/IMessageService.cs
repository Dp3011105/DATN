using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task<Message?> GetByIdAsync(int id);
        Task AddAsync(Message entity);
        Task UpdateAsync(Message entity);
        Task DeleteAsync(int id);
    }
}