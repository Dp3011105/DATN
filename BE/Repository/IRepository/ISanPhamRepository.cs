using BE.models;

namespace Repository.IRepository
{
    public interface ISanPhamRepository
    {
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<SanPham?> GetByIdAsync(int id);
        Task AddAsync(SanPham entity);
        Task UpdateAsync(SanPham entity);
        Task DeleteAsync(int id);
    }
}