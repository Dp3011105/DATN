using BE.models;

namespace Repository.IRepository
{
    public interface ISanPhamToppingRepository
    {
        Task<IEnumerable<SanPhamTopping>> GetAllAsync();
        Task<SanPhamTopping?> GetByIdAsync(int id);
        Task AddAsync(SanPhamTopping entity);
        Task UpdateAsync(SanPhamTopping entity);
        Task DeleteAsync(int id);
    }
}