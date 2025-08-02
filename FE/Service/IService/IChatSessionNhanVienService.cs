using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IChatSessionNhanVienService
    {
        Task<IEnumerable<ChatSessionNhanVien>> GetAllAsync();
        Task<ChatSessionNhanVien?> GetByIdAsync(int id);
        Task AddAsync(ChatSessionNhanVien entity);
        Task UpdateAsync(ChatSessionNhanVien entity);
        Task DeleteAsync(int id);
    }
}