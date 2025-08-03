using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IGioHang_ChiTietService
    {
        Task<IEnumerable<GioHang_ChiTiet>> GetAllAsync();
        Task<GioHang_ChiTiet?> GetByIdAsync(int id);
        Task AddAsync(GioHang_ChiTiet entity);
        Task UpdateAsync(GioHang_ChiTiet entity);
        Task DeleteAsync(int id);
    }
}