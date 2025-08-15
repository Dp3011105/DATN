using BE.DTOs;
using BE.models;

namespace Repository.IRepository
{
    public interface ITaiKhoanVaiTroRepository
    {
        Task<IEnumerable<TaiKhoanVaiTroDTO>> GetAllTaiKhoanVaiTroAsync();
        Task<TaiKhoanVaiTroDTO?> GetTaiKhoanVaiTroByIdAsync(int idTaiKhoan, int idVaiTro);
        Task CreateTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro);
        Task UpdateTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro);
        Task DeleteTaiKhoanVaiTroAsync(int idTaiKhoan, int idVaiTro);
    }
}
