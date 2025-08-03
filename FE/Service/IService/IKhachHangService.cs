using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IKhachHangService
    {
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang?> GetByIdAsync(int id);
        Task AddAsync(KhachHang entity);
        Task UpdateAsync(KhachHang entity);
        Task DeleteAsync(int id);
    }
}