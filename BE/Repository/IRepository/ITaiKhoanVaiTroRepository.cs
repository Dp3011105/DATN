using BE.models;

namespace Repository.IRepository
{
    public interface ITaiKhoanVaiTroRepository
    {
        Task<IEnumerable<TaiKhoanVaiTro>> GetAllAsync();
        Task<TaiKhoanVaiTro?> GetByIdAsync(int id);
        Task AddAsync(TaiKhoanVaiTro entity);
        Task UpdateAsync(TaiKhoanVaiTro entity);
        Task DeleteAsync(int id);
    }
}