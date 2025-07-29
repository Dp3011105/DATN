using BE.models;

namespace Repository.IRepository
{
    public interface IDiaChiRepository
    {
        Task<IEnumerable<DiaChi>> GetAllAsync();
        Task<DiaChi?> GetByIdAsync(int id);
        Task AddAsync(DiaChi entity);
        Task UpdateAsync(DiaChi entity);
        Task DeleteAsync(int id);
    }
}