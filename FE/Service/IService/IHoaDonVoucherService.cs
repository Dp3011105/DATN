using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IHoaDonVoucherService
    {
        Task<IEnumerable<HoaDonVoucher>> GetAllAsync();
        Task<HoaDonVoucher?> GetByIdAsync(int id);
        Task AddAsync(HoaDonVoucher entity);
        Task UpdateAsync(HoaDonVoucher entity);
        Task DeleteAsync(int id);
    }
}