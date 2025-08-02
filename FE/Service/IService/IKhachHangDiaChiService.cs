using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IKhachHangDiaChiService
    {
        Task<IEnumerable<KhachHangDiaChi>> GetAllAsync();
        Task<KhachHangDiaChi?> GetByIdAsync(int id);
        Task AddAsync(KhachHangDiaChi entity);
        Task UpdateAsync(KhachHangDiaChi entity);
        Task DeleteAsync(int id);
    }
}