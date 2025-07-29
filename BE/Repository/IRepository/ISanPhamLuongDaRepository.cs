using BE.models;

namespace Repository.IRepository
{
    public interface ISanPhamLuongDaRepository
    {
        Task<IEnumerable<SanPhamLuongDa>> GetAllAsync();
        Task<SanPhamLuongDa?> GetByIdAsync(int id);
        Task AddAsync(SanPhamLuongDa entity);
        Task UpdateAsync(SanPhamLuongDa entity);
        Task DeleteAsync(int id);
    }
}