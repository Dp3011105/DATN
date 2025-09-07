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

        // --- DANH SÁCH ---
        Task<IEnumerable<HoaDonDTO>> GetAllAsync();          // DTO cho UI list
        Task<IEnumerable<HoaDon>> GetAllEntitiesAsync();     // Entity cho FE cũ

        // --- CẬP NHẬT TRẠNG THÁI ---
        Task<bool> UpdateTrangThaiAsync(int id, string trangThai, string? lyDoHuy);

        // --- NEW: HỦY + HOÀN TRẢ TỒN KHO ---
        /// <summary>
        /// Hủy đơn (đổi trạng thái sang Huy_Don, lưu lý do) và cộng trả tồn kho cho các chi tiết được chọn.
        /// selections: (ID_HoaDon_ChiTiet, quantity_to_restock)
        /// </summary>
        Task<bool> CancelWithRestockAsync(
            int hoaDonId,
            string lyDoHuy,
            IEnumerable<(int chiTietId, int quantity)> selections
        );
    }
}
