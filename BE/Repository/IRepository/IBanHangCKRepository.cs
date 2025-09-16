using BE.DTOs;
using BE.models;

namespace BE.Repository.IRepository
{
    public interface IBanHangCKRepository
    {

        Task<IEnumerable<HinhThucThanhToanDTO>> GetAllHinhThucThanhToan();
        Task<HoaDonBanHangCKDTO> CheckOutTk(HoaDonBanHangCKDTO hoaDonDto);

        // xử lý vấn đề voucher 

        Task<List<VoucherBanHangCKDto>> GetAllVouchersAsync();
        Task<Voucher> GetVoucherByIdAsync(int id);
        Task<bool> UpdateVoucherAsync(Voucher voucher);



    }
}
