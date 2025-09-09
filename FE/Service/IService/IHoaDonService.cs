using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

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

        // --- Màn list (tùy bạn dùng DTO hay projection) ---
        Task<IEnumerable<object>> GetAllListAsync();

        // Cập nhật trạng thái nhanh + lý do hủy
        Task<bool> UpdateTrangThaiAsync(int hoaDonId, string trangThaiDb, string? lyDoHuy);

        // NEW: Hủy + restock
        Task<bool> CancelWithRestockAsync(int hoaDonId, string lyDo, List<(int chiTietId, int soLuong)> selections);
    }
}
