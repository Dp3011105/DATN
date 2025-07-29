using BE.models;

namespace Repository.IRepository
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<Voucher?> GetByIdAsync(int id);
        Task AddAsync(Voucher entity);
        Task UpdateAsync(Voucher entity);
        Task DeleteAsync(int id);
    }
}