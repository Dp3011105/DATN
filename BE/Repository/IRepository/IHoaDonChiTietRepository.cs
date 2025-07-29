using BE.models;

namespace Repository.IRepository
{
    public interface IHoaDonChiTietRepository
    {
        Task<IEnumerable<HoaDonChiTiet>> GetAllAsync();
        Task<HoaDonChiTiet?> GetByIdAsync(int id);
        Task AddAsync(HoaDonChiTiet entity);
        Task UpdateAsync(HoaDonChiTiet entity);
        Task DeleteAsync(int id);
    }
}