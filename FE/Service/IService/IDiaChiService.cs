using BE.models;

namespace Service.IService
{
    public interface IDiaChiService
    {
        Task<IEnumerable<DiaChi>> GetAllAsync();
        Task<DiaChi?> GetByIdAsync(int id);
        Task AddAsync(DiaChi entity);
        Task UpdateAsync(DiaChi entity);
        Task DeleteAsync(int id);
    }
}