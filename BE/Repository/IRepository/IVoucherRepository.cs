using BE.models;

namespace BE.Repository.IRepository
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<Voucher?> GetByIdAsync(int id);
        Task<Voucher?> GetByCodeAsync(string code);
        Task<Voucher> CreateAsync(Voucher voucher);
        Task<Voucher> UpdateAsync(Voucher voucher);
        Task<bool> ActivateAsync(int id);  // THÊM: Method kích hoạt voucher
        Task<bool> DeactivateAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        // Thêm 2 method mới:
        Task<IEnumerable<Voucher>> GetActiveVouchersAsync();
        Task<bool> CanUseVoucherAsync(string code, decimal orderAmount);
    }
}