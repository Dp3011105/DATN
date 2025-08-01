using BE.models;

namespace Repository.IRepository
{
    public interface IChatSessionNhanVienRepository
    {
        Task<IEnumerable<ChatSessionNhanVien>> GetAllAsync();
        Task<ChatSessionNhanVien?> GetByIdAsync(int id);
        Task AddAsync(ChatSessionNhanVien entity);
        Task UpdateAsync(ChatSessionNhanVien entity);
        Task DeleteAsync(int id);
    }
}