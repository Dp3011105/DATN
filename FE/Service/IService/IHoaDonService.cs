using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;
using BE.DTOs;

namespace Service.IService
{
    public interface IHoaDonService
    {
        // --- GIỮ NGUYÊN ---
        Task<IEnumerable<HoaDon>> GetAllAsync();
        Task<HoaDon> GetByIdAsync(int id);
        Task AddAsync(HoaDon entity);
        Task UpdateAsync(int id, HoaDon entity);
        Task DeleteAsync(int id);

        // --- THÊM MỚI ---
        // Lấy danh sách cho màn list (DTO có đủ Loai_Hoa_Don, ThanhToan, DiaChi)
        Task<IEnumerable<HoaDonDTO>> GetAllListAsync();

        // Cập nhật trạng thái nhanh + lý do hủy
        Task<bool> UpdateTrangThaiAsync(int hoaDonId, string trangThaiDb, string? lyDoHuy);
    }
}
