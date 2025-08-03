using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IHinhThucThanhToanService
    {
        Task<IEnumerable<HinhThucThanhToan>> GetAllAsync();
        Task<HinhThucThanhToan?> GetByIdAsync(int id);
        Task AddAsync(HinhThucThanhToan entity);
        Task UpdateAsync(HinhThucThanhToan entity);
        Task DeleteAsync(int id);
    }
}