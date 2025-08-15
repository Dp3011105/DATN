using BE.models;

namespace Repository.IRepository
{
    public interface IVaiTroRepository
    {
        Task<IEnumerable<VaiTro>> GetAllAsync();
        Task<VaiTro> GetByIdAsync(int id);
        Task AddAsync(VaiTro entity);
        Task UpdateAsync(int id,VaiTro entity);
        Task DeleteAsync(int id);
    }
}