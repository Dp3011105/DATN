//using BE.models;
//using System.Collections.Generic;

//namespace BE.Repository.IRepository
//{
//    public interface IDonHangTKRepository
//    {
//        Task<List<HoaDon>> GetHoaDonsByKhachHangAsync(int idKhachHang);
//        Task<HoaDon> GetHoaDonByIdAsync(int idHoaDon);
//        Task UpdateHoaDonAsync(HoaDon hoaDon);


//    }
//}




using BE.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE.Repository.IRepository
{
    public interface IDonHangTKRepository
    {
        Task<List<HoaDon>> GetHoaDonsByKhachHangAsync(int idKhachHang);
        Task<HoaDon> GetHoaDonByIdAsync(int idHoaDon);
        Task UpdateHoaDonAsync(HoaDon hoaDon);
        Task<HoaDonVoucher> GetHoaDonVoucherByHoaDonIdAsync(int idHoaDon);
        Task<KhachHangVoucher> GetKhachHangVoucherAsync(int idKhachHang, int idVoucher);
        Task UpdateKhachHangVoucherAsync(KhachHangVoucher khachHangVoucher);
    }
}