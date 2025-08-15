using BE.models;

namespace Repository.IRepository
{
    public interface ITaiKhoanRepository
    {
        Task<IEnumerable<TaiKhoan>> GetAllAsync();
        Task<TaiKhoan> GetByIdAsync(int id);
        Task AddAsync(TaiKhoan entity);
        Task UpdateAsync(int �d,TaiKhoan entity);
        Task DeleteAsync(int id);
    }
}