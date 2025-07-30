using BE.models;

namespace Repository.IRepository
{
    public interface IHinhThucThanhToanRepository
    {
        Task<IEnumerable<HinhThucThanhToan>> GetAllAsync();
        Task<HinhThucThanhToan?> GetByIdAsync(int id);
        Task AddAsync(HinhThucThanhToan entity);
        Task UpdateAsync(HinhThucThanhToan entity);
        Task DeleteAsync(int id);
    }
}