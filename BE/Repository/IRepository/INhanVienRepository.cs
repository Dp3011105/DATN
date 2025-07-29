using BE.models;

namespace Repository.IRepository
{
    public interface INhanVienRepository
    {
        Task<IEnumerable<NhanVien>> GetAllAsync();
        Task<NhanVien?> GetByIdAsync(int id);
        Task AddAsync(NhanVien entity);
        Task UpdateAsync(NhanVien entity);
        Task DeleteAsync(int id);
    }
}