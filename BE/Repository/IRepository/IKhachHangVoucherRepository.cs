using BE.models;

namespace Repository.IRepository
{
    public interface IKhachHangVoucherRepository
    {
        Task<IEnumerable<KhachHangVoucher>> GetAllAsync();
        Task<KhachHangVoucher?> GetByIdAsync(int id);
        Task AddAsync(KhachHangVoucher entity);
        Task UpdateAsync(KhachHangVoucher entity);
        Task DeleteAsync(int id);
    }
}