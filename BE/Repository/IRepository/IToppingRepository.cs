using BE.models;

namespace Repository.IRepository
{
    public interface IToppingRepository
    {
        Task<IEnumerable<Topping>> GetAllAsync();
        Task<Topping?> GetByIdAsync(int id);
        Task AddAsync(Topping entity);
        Task UpdateAsync(Topping entity);
        Task DeleteAsync(int id);
    }
}