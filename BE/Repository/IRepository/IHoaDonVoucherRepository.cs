using BE.models;

namespace Repository.IRepository
{
    public interface IHoaDonVoucherRepository
    {
        Task<IEnumerable<HoaDonVoucher>> GetAllAsync();
        Task<HoaDonVoucher?> GetByIdAsync(int id);
        Task AddAsync(HoaDonVoucher entity);
        Task UpdateAsync(HoaDonVoucher entity);
        Task DeleteAsync(int id);
    }
}