using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IDiemDanhService
    {
        Task<IEnumerable<DiemDanh>> GetAllAsync();
        Task<DiemDanh?> GetByIdAsync(int id);
        Task AddAsync(DiemDanh entity);
        Task UpdateAsync(DiemDanh entity);
        Task DeleteAsync(int id);
    }
}