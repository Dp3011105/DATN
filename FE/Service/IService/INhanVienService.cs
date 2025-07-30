using BE.models;

namespace Service.IService
{
    public interface INhanVienService
    {
        Task<IEnumerable<NhanVien>> GetAllAsync();
        Task<NhanVien?> GetByIdAsync(int id);
        Task AddAsync(NhanVien entity);
        Task UpdateAsync(NhanVien entity);
        Task DeleteAsync(int id);
    }
}