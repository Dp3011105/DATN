using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IGio_HangService
    {
        Task<IEnumerable<Gio_Hang>> GetAllAsync();
        Task<Gio_Hang?> GetByIdAsync(int id);
        Task AddAsync(Gio_Hang entity);
        Task UpdateAsync(Gio_Hang entity);
        Task DeleteAsync(int id);
    }
}