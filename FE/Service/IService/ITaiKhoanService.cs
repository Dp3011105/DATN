using BE.models;

namespace Service.IService
{
    public interface ITaiKhoanService
    {
        Task<IEnumerable<TaiKhoan>> GetAllAsync();
        Task<TaiKhoan?> GetByIdAsync(int id);
        Task AddAsync(TaiKhoan entity);
        Task UpdateAsync(TaiKhoan entity);
        Task DeleteAsync(int id);
    }
}