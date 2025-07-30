using BE.models;

namespace Repository.IRepository
{
    public interface IKhachHangDiaChiRepository
    {
        Task<IEnumerable<KhachHangDiaChi>> GetAllAsync();
        Task<KhachHangDiaChi?> GetByIdAsync(int id);
        Task AddAsync(KhachHangDiaChi entity);
        Task UpdateAsync(KhachHangDiaChi entity);
        Task DeleteAsync(int id);
    }
}