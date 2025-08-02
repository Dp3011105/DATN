using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface ISanPhamToppingService
    {
        Task<IEnumerable<SanPhamTopping>> GetAllAsync();
        Task<SanPhamTopping?> GetByIdAsync(int id);
        Task AddAsync(SanPhamTopping entity);
        Task UpdateAsync(SanPhamTopping entity);
        Task DeleteAsync(int id);
    }
}