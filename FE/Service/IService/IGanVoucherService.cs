using BE.models;
using FE.Models;

namespace FE.Service
{
    public interface IGanVoucherService
    {
        Task<List<KhachHang>> GetAllKhachHangAsync();
        Task<List<Voucher>> GetAllVouchersAsync();
        Task<GanVoucherResult> GanVoucherAsync(GanVoucherViewModel viewModel); // Thay đổi return type
        Task<List<KhachHangVoucher>> GetVouchersByKhachHangAsync(int khachHangId);
        Task<List<KhachHang>> GetTop10VipAsync();
    }
}