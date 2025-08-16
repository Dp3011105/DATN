using BE.DTOs;
namespace BE.Repository.IRepository
{
    public interface IQuanLyPhanQuyenNhanVienRepository
    {
        Task<List<NhanVienVoiTaiKhoan>> LayDanhSachNhanVienCoVaiTroAsync();
        Task<List<NhanVienVoiTaiKhoan>> LayDanhSachNhanVienKhongCoVaiTroAsync();
        Task GanVaiTroChoTaiKhoanAsync(int accountId, List<int> roleIds);
        Task CapNhatVaiTroChoTaiKhoanAsync(int accountId, List<int> roleIds);
    }
}
