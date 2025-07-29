using BE.models;

namespace Service.IService
{
    public interface ISizeService
    {
        Task<IEnumerable<Size>> GetAllAsync();
        Task<Size?> GetByIdAsync(int id);
        Task AddAsync(Size entity);
        Task UpdateAsync(Size entity);
        Task DeleteAsync(int id);
    }
}