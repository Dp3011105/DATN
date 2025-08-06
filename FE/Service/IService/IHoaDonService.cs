using BE.DTOs;
using BE.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IHoaDonService
    {
        Task<IEnumerable<HoaDon>> GetAllAsync();
        Task<HoaDon> GetByIdAsync(int id);
        Task AddAsync(HoaDon entity);
        Task UpdateAsync(int id,HoaDon entity);
        Task DeleteAsync(int id);
    }
}