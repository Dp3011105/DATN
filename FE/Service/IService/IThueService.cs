using BE.models;

namespace Service.IService
{
    public interface IThueService
    {
        Task<IEnumerable<Thue>> GetAllAsync();
        Task<Thue?> GetByIdAsync(int id);
        Task AddAsync(Thue entity);
        Task UpdateAsync(Thue entity);
        Task DeleteAsync(int id);
    }
}