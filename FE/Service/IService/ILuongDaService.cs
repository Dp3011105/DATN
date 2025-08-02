using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface ILuongDaService
    {
        Task<IEnumerable<LuongDa>> GetAllAsync();
        Task<LuongDa?> GetByIdAsync(int id);
        Task AddAsync(LuongDa entity);
        Task UpdateAsync(LuongDa entity);
        Task DeleteAsync(int id);
    }
}