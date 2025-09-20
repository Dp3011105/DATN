using BE.DTOs;
using BE.models;
using BE.Service;

namespace BE.Repository.IRepository
{
    public interface IBanHangCKRepository
    {

        Task<IEnumerable<HinhThucThanhToanDTO>> GetAllHinhThucThanhToan();
        Task<HoaDonBanHangCKDTO> CheckOutTk(HoaDonBanHangCKDTO hoaDonDto, EmailService emailService);

        // xử lý vấn đề voucher 

        Task<List<VoucherBanHangCKDto>> GetAllVouchersAsync();
        Task<Voucher> GetVoucherByIdAsync(int id);
        Task<bool> UpdateVoucherAsync(Voucher voucher);



    }
}
