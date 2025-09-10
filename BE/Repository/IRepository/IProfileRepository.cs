using BE.DTOs;
using BE.models;

namespace BE.Repository.IRepository
{
    public interface IProfileRepository
    {
        Task<KhachHang> GetKhachHangByIdAsync(int khachHangId);
        Task<bool> UpdateKhachHangAsync(KhachHang khachHang);
        Task<List<DiaChi>> GetDiaChiByKhachHangIdAsync(int khachHangId);
        Task<DiaChi> GetDiaChiByIdAsync(int diaChiId);
        Task<bool> AddDiaChiAsync(DiaChi diaChi, int khachHangId);
        Task<bool> UpdateDiaChiAsync(DiaChi diaChi);
        Task<bool> DeleteDiaChiAsync(int diaChiId, int khachHangId);
        Task<int> SaveChangesAsync();
    }
}