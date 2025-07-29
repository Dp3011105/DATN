using BE.models;

namespace Service.IService
{
    public interface IHoaDonChiTietService
    {
        Task<IEnumerable<HoaDonChiTiet>> GetAllAsync();
        Task<HoaDonChiTiet?> GetByIdAsync(int id);
        Task AddAsync(HoaDonChiTiet entity);
        Task UpdateAsync(HoaDonChiTiet entity);
        Task DeleteAsync(int id);
    }
}