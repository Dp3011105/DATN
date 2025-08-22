using FE.Models;
namespace FE.Service.IService
{
    public interface IKhuyenMaiService
    {
        Task<List<KhuyenMaiCrud>> GetAllPromotionsAsync();
        Task<KhuyenMaiCrud> GetPromotionByIdAsync(int id);
        Task<bool> AddPromotionAsync(KhuyenMaiCrud khuyenMai);
        Task<bool> UpdatePromotionAsync(KhuyenMaiCrud khuyenMai);
        Task<bool> DeletePromotionAsync(int id);
    }
}
