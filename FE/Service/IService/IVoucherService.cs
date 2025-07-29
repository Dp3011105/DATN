using BE.models;

namespace Service.IService
{
    public interface IVoucherService
    {
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<Voucher?> GetByIdAsync(int id);
        Task AddAsync(Voucher entity);
        Task UpdateAsync(Voucher entity);
        Task DeleteAsync(int id);
    }
}