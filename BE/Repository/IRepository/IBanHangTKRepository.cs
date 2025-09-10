using BE.DTOs;

namespace BE.Repository.IRepository
{
    public interface IBanHangTKRepository
    {
        Task<List<DiaChiBanHangDTO>> GetActiveDiaChiByKhachHangId(int idKhachHang);
        Task<bool> UpdateDiaChi(int idKhachHang, int idDiaChi, UpdateDiaChiBanHangDTO dto);
        Task<int> AddDiaChiForKhachHang(int idKhachHang, CreateDiaChiBanHangDTO dto);
        Task<IEnumerable<VoucherBanHangDTO>> GetVouchersByKhachHang(int idKhachHang);
        Task<IEnumerable<HinhThucThanhToanDTO>> GetAllHinhThucThanhToan();
        Task<HoaDonBanHangTKDTO> CheckOutTk(HoaDonBanHangTKDTO hoaDonDto);

        // Methods mới cho So_Dien_Thoai
        Task<bool> KiemTraCoSoDienThoaiAsync(int idKhachHang);
        Task<string> LaySoDienThoaiAsync(int idKhachHang);
        Task ThemSoDienThoaiAsync(int idKhachHang, string soDienThoai);
        Task CapNhatSoDienThoaiAsync(int idKhachHang, string soDienThoaiMoi);


    }
}
