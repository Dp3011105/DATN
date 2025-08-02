using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface ILichSuHoaDonService
    {
        Task<IEnumerable<LichSuHoaDon>> GetAllAsync();
        Task<LichSuHoaDon?> GetByIdAsync(int id);
        Task AddAsync(LichSuHoaDon entity);
        Task UpdateAsync(LichSuHoaDon entity);
        Task DeleteAsync(int id);
    }
}