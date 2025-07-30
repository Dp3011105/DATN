using BE.models;

namespace Repository.IRepository
{
    public interface IHoaDonChiTietThueRepository
    {
        Task<IEnumerable<HoaDonChiTietThue>> GetAllAsync();
        Task<HoaDonChiTietThue?> GetByIdAsync(int id);
        Task AddAsync(HoaDonChiTietThue entity);
        Task UpdateAsync(HoaDonChiTietThue entity);
        Task DeleteAsync(int id);
    }
}