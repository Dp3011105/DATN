using BE.models;

namespace Service.IService
{
    public interface ITaiKhoanVaiTroService
    {
        Task<IEnumerable<TaiKhoanVaiTro>> GetAllAsync();
        Task<TaiKhoanVaiTro?> GetByIdAsync(int id);
        Task AddAsync(TaiKhoanVaiTro entity);
        Task UpdateAsync(TaiKhoanVaiTro entity);
        Task DeleteAsync(int id);
    }
}