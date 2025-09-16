using BE.models;

namespace Repository.IRepository
{
    public interface ITaiKhoanRepository // Hưng Repository cho tài khoản người dùng
    {
        Task<IEnumerable<TaiKhoan>> GetAllAsync();
        Task<TaiKhoan> GetByIdAsync(int id);
        Task AddAsync(TaiKhoan entity);
        Task UpdateAsync(int id,TaiKhoan entity);
        Task DeleteAsync(int id);
    }
}