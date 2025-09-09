using FE.Models;

namespace FE.Service.IService
{
    public interface IQLDonHangTkService
    {
        Task<List<DonHangTK>> GetOrdersAsync(int idKhachHang);
        Task<bool> CancelOrderAsync(DonHangTKCancelRequest request);
    }
}
