using BE.models;

namespace Service.IService
{
    public interface IHoaDonService
    {
        Task<IEnumerable<HoaDon>> GetAllAsync();
        Task<HoaDon?> GetByIdAsync(int id);
        Task AddAsync(HoaDon entity);
        Task UpdateAsync(HoaDon entity);
        Task DeleteAsync(int id);
    }
}