using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IVaiTroService
    {
        Task<IEnumerable<VaiTro>> GetAllAsync();
        Task<VaiTro> GetByIdAsync(int id);
        Task AddAsync(VaiTro entity);
        Task UpdateAsync(int id,VaiTro entity);
        Task DeleteAsync(int id);
    }
}