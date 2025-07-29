using BE.models;

namespace Repository.IRepository
{
    public interface IHoaDonRepository
    {
        Task<IEnumerable<HoaDon>> GetAllAsync();
        Task<HoaDon?> GetByIdAsync(int id);
        Task AddAsync(HoaDon entity);
        Task UpdateAsync(HoaDon entity);
        Task DeleteAsync(int id);
    }
}