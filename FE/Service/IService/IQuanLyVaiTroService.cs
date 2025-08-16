using FE.Models;

namespace FE.Service.IService
{
    public interface IQuanLyVaiTroService
    {
        Task<List<TaiKhoanNhanVien>> GetNhanVienKhongVaiTro();
        Task<List<TaiKhoanNhanVien>> GetNhanVienCoVaiTro();
        Task<List<VaiTro>> GetAllVaiTro();
        Task GanVaiTro(GanVaiTroRequest request);
        Task CapNhatVaiTro(GanVaiTroRequest request);
    }
}
