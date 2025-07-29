using BE.models;

namespace Repository.IRepository
{
    public interface ISanPhamDoNgotRepository
    {
        Task<IEnumerable<SanPhamDoNgot>> GetAllAsync();
        Task<SanPhamDoNgot?> GetByIdAsync(int id);
        Task AddAsync(SanPhamDoNgot entity);
        Task UpdateAsync(SanPhamDoNgot entity);
        Task DeleteAsync(int id);
    }
}