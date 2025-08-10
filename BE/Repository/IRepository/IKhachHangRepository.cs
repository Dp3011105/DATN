using BE.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IKhachHangRepository
    {
        Task<IEnumerable<KhachHangDTO>> GetAllKhachHang();
        Task<KhachHangDTO> GetKhachHangById(int id);
        Task AddKhachHang(KhachHangDTO entity);
        Task UpdateKhachHang(int id, KhachHangDTO entity);
        Task DeleteKhachHang(int id);
    }
}