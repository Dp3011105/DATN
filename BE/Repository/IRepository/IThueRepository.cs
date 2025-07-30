using BE.models;

namespace Repository.IRepository
{
    public interface IThueRepository
    {
        Task<IEnumerable<Thue>> GetAllAsync();
        Task<Thue?> GetByIdAsync(int id);
        Task AddAsync(Thue entity);
        Task UpdateAsync(Thue entity);
        Task DeleteAsync(int id);
    }
}