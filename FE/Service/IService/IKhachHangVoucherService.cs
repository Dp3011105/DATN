using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IKhachHangVoucherService
    {
        Task<IEnumerable<KhachHangVoucher>> GetAllAsync();
        Task<KhachHangVoucher?> GetByIdAsync(int id);
        Task AddAsync(KhachHangVoucher entity);
        Task UpdateAsync(KhachHangVoucher entity);
        Task DeleteAsync(int id);
    }
}