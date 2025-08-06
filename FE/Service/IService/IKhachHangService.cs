using System.Collections.Generic;
using System.Threading.Tasks;
using BE.DTOs;

namespace Service.IService
{
    public interface IKhachHangService
    {
        Task<IEnumerable<KhachHangDTO>> GetAllKhachHang();
        Task<KhachHangDTO> GetKhachHangById(int id);
        Task AddKhachHang(KhachHangDTO entity);
        Task UpdateKhachHang(int id, KhachHangDTO entity);
        Task DeleteKhachHang(int id);
    }
}