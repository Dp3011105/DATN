using BE.DTOs;
using BE.models;

namespace Service.IService
{
    public interface ITaiKhoanVaiTroService
    {
        Task<IEnumerable<TaiKhoanVaiTroDTO>> GetAllTaiKhoanVaiTroAsync();
        Task<TaiKhoanVaiTroDTO> GetTaiKhoanVaiTroByIdAsync(int idTaiKhoan, int idVaiTro);
        Task CreateTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro);
        Task UpdateTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro);
        Task DeleteTaiKhoanVaiTroAsync(int idTaiKhoan, int idVaiTro);
        Task<IEnumerable<TaiKhoan>> GetAllTaiKhoanNhanVienAsync();
        Task<IEnumerable<VaiTro>> GetAllVaiTroAsync();
    }
}
