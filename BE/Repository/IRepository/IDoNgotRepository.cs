using BE.models;

namespace Repository.IRepository
{
    public interface IDoNgotRepository
    {
        Task<IEnumerable<DoNgot>> GetAllAsync();
        Task<DoNgot?> GetByIdAsync(int id);
        Task AddAsync(DoNgot entity);
        Task UpdateAsync(DoNgot entity);
        Task DeleteAsync(int id);
    }
}