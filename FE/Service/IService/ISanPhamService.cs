using BE.models;

namespace Service.IService
{
    public interface ISanPhamService
    {
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<SanPham?> GetByIdAsync(int id);
        Task AddAsync(SanPham entity);
        Task UpdateAsync(SanPham entity);
        Task DeleteAsync(int id);
    }
}