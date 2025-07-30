using BE.models;

namespace Service.IService
{
    public interface IHoaDonChiTietThueService
    {
        Task<IEnumerable<HoaDonChiTietThue>> GetAllAsync();
        Task<HoaDonChiTietThue?> GetByIdAsync(int id);
        Task AddAsync(HoaDonChiTietThue entity);
        Task UpdateAsync(HoaDonChiTietThue entity);
        Task DeleteAsync(int id);
    }
}