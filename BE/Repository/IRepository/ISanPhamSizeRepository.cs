using BE.models;

namespace Repository.IRepository
{
    public interface ISanPhamSizeRepository
    {
        Task<IEnumerable<SanPhamSize>> GetAllAsync();
        Task<SanPhamSize?> GetByIdAsync(int id);
        Task AddAsync(SanPhamSize entity);
        Task UpdateAsync(SanPhamSize entity);
        Task DeleteAsync(int id);
    }
}