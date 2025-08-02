using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IDoNgotService
    {
        Task<IEnumerable<DoNgot>> GetAllAsync();
        Task<DoNgot?> GetByIdAsync(int id);
        Task AddAsync(DoNgot entity);
        Task UpdateAsync(DoNgot entity);
        Task DeleteAsync(int id);
    }
}