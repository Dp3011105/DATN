using BE.models;

namespace Repository.IRepository
{
    public interface IDiemDanhRepository
    {
        Task<IEnumerable<DiemDanh>> GetAllAsync();
        Task<DiemDanh?> GetByIdAsync(int id);
        Task AddAsync(DiemDanh entity);
        Task UpdateAsync(DiemDanh entity);
        Task DeleteAsync(int id);
    }
}