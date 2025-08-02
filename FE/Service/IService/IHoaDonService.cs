using BE.DTOs;
using BE.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IHoaDonService
    {
        Task<IEnumerable<HoaDonDTO>> GetAllAsync();
        Task<HoaDon?> GetByIdAsync(int id);
        Task AddAsync(HoaDon entity);
        Task UpdateAsync(HoaDon entity);
        Task DeleteAsync(int id);
    }
}