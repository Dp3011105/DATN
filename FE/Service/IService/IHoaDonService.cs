
using BE.DTOs;
using BE.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IHoaDonService
    {

        // Task<IEnumerable<HoaDonDTO>> GetAllAsync(); Hiện mình không thấy HoaDonDTO nên command lại để tránh lỗi 
        Task<HoaDon?> GetByIdAsync(int id);
        Task AddAsync(HoaDon entity);
        Task UpdateAsync(HoaDon entity);
        Task DeleteAsync(int id);
    }
}