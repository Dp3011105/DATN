using FE.Models;

namespace FE.Service.IService
{
    public interface IPromotionService// dùng cho chức năng áp dụng khuyến mãi cho sản phẩm 
    {
        Task<List<Promotion>> GetActivePromotionsAsync();
        Task<List<Product>> GetProductsByPromotionAsync();
        Task<bool> AssignPromotionAsync(AssignPromotionRequest request);
    }
}
