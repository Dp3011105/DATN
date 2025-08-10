using BE.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface INhanVienService
    {
        Task<IEnumerable<NhanVien>> GetAllAsync();
        Task<NhanVien> GetByIdAsync(int id);
        Task AddAsync(NhanVien entity);
        Task UpdateAsync(int id, NhanVien entity);
        Task DeleteAsync(int id);
    }
}