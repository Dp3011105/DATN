using BE.models;

namespace Repository.IRepository
{
    public interface IGio_HangRepository
    {
        Task<IEnumerable<Gio_Hang>> GetAllAsync();
        Task<Gio_Hang?> GetByIdAsync(int id);
        Task AddAsync(Gio_Hang entity);
        Task UpdateAsync(Gio_Hang entity);
        Task DeleteAsync(int id);
    }
}