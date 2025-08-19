using BE.DTOs.Requests;
using BE.models;

namespace BE.Repository.IRepository
{
    public interface IGanVoucherRepository
    {
        Task<List<KhachHang>> GetAllKhachHangAsync();
        Task<List<Voucher>> GetAllVouchersAsync();
        Task<string> GanVoucherAsync(GanVoucherRequest request);
        Task<List<KhachHangVoucher>> GetVouchersByKhachHangAsync(int khachHangId);
        Task<bool> IsVoucherAssignedToCustomerAsync(int khachHangId, int voucherId);

        // Thêm method mới
        Task<List<KhachHang>> GetTop10KhachHangVipAsync();
    }
}