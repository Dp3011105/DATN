using BE.models;

namespace Repository.IRepository
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task<Message?> GetByIdAsync(int id);
        Task AddAsync(Message entity);
        Task UpdateAsync(Message entity);
        Task DeleteAsync(int id);
    }
}