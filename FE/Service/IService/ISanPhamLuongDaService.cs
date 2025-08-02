using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface ISanPhamLuongDaService
    {
        Task<IEnumerable<SanPhamLuongDa>> GetAllAsync();
        Task<SanPhamLuongDa?> GetByIdAsync(int id);
        Task AddAsync(SanPhamLuongDa entity);
        Task UpdateAsync(SanPhamLuongDa entity);
        Task DeleteAsync(int id);
    }
}