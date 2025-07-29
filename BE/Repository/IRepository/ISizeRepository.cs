using BE.models;

namespace Repository.IRepository
{
    public interface ISizeRepository
    {
        Task<IEnumerable<Size>> GetAllAsync();
        Task<Size?> GetByIdAsync(int id);
        Task AddAsync(Size entity);
        Task UpdateAsync(Size entity);
        Task DeleteAsync(int id);
    }
}