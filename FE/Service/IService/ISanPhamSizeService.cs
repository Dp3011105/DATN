using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface ISanPhamSizeService
    {
        Task<IEnumerable<SanPhamSize>> GetAllAsync();
        Task<SanPhamSize?> GetByIdAsync(int id);
        Task AddAsync(SanPhamSize entity);
        Task UpdateAsync(SanPhamSize entity);
        Task DeleteAsync(int id);
    }
}