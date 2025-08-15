using FE.Models;

namespace FE.Service.IService
{
    public interface ICartService// dùng cho giỏ hàng để get id khách hang 
    {
        Task<GioHang> GetCartByUserIdAsync(int userId);
        Task<bool> AddToCartAsync(object cartItem);

    }
}
