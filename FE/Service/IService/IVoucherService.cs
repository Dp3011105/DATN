using FE.Models;

namespace FE.Service.IService
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherViewModel>> GetAllAsync();
        Task<VoucherViewModel?> GetByIdAsync(int id);
        Task<VoucherViewModel?> GetByCodeAsync(string code);
        Task<VoucherViewModel> CreateAsync(VoucherViewModel voucher);
        Task<VoucherViewModel> UpdateAsync(VoucherViewModel voucher);
        Task<bool> DeactivateAsync(int id);
        Task<bool> CheckCodeExistsAsync(string code, int? excludeId = null);

        // THÊM: 2 method mới
        Task<IEnumerable<VoucherViewModel>> GetActiveVouchersAsync();
        Task<object> ValidateVoucherAsync(string code, decimal orderAmount);
        Task<bool> BulkActivateAsync(List<int> voucherIds);
        Task<bool> BulkDeactivateAsync(List<int> voucherIds);
    }
}