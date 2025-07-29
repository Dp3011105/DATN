using BE.models;

namespace Repository.IRepository
{
    public interface ILichSuHoaDonRepository
    {
        Task<IEnumerable<LichSuHoaDon>> GetAllAsync();
        Task<LichSuHoaDon?> GetByIdAsync(int id);
        Task AddAsync(LichSuHoaDon entity);
        Task UpdateAsync(LichSuHoaDon entity);
        Task DeleteAsync(int id);
    }
}