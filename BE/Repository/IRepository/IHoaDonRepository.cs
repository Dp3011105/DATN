namespace BE.Repository.IRepository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BE.DTOs;
    using BE.models;

    public interface IHoaDonRepository
    {
        // --- GIỮ NGUYÊN ---
        Task<HoaDon> GetByIdAsync(int id);
        Task AddAsync(HoaDon entity);
        Task UpdateAsync(int id, HoaDon entity);
        Task DeleteAsync(int id);

        // --- THÊM MỚI ---
        // 1) Danh sách dạng DTO (cho màn list)
        Task<IEnumerable<HoaDonDTO>> GetAllAsync();

        // 2) Danh sách entity đầy đủ (để giữ tương thích các màn FE cũ)
        Task<IEnumerable<HoaDon>> GetAllEntitiesAsync();

        // 3) Cập nhật trạng thái + lý do hủy
        Task<bool> UpdateTrangThaiAsync(int id, string trangThai, string? lyDoHuy);
    }
}
