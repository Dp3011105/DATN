using BE.models;

namespace Service.IService
{
    public interface ISanPhamDoNgotService
    {
        Task<IEnumerable<SanPhamDoNgot>> GetAllAsync();
        Task<SanPhamDoNgot?> GetByIdAsync(int id);
        Task AddAsync(SanPhamDoNgot entity);
        Task UpdateAsync(SanPhamDoNgot entity);
        Task DeleteAsync(int id);
    }
}