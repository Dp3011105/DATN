using BE.models;

namespace Repository.IRepository
{
    public interface ILuongDaRepository
    {
        Task<IEnumerable<LuongDa>> GetAllAsync();
        Task<LuongDa?> GetByIdAsync(int id);
        Task AddAsync(LuongDa entity);
        Task UpdateAsync(LuongDa entity);
        Task DeleteAsync(int id);
    }
}