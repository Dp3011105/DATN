using BE.models;

namespace Repository.IRepository
{
    public interface IKhachHangRepository
    {
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang?> GetByIdAsync(int id);
        Task AddAsync(KhachHang entity);
        Task UpdateAsync(KhachHang entity);
        Task DeleteAsync(int id);
    }
}