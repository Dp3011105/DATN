using BE.DTOs;
using BE.models;

namespace BE.Repository.IRepository
{
    public interface ISanPhamRepository
    {
        Task<List<SanPham>> GetAllWithDetailsAsync();
        Task<SanPham> GetByIdWithDetailsAsync(int id);
        Task<SanPhamDTO> CreateSanPhamAsync(SanPhamDTO sanPhamDTO, string imagePath);
        Task<SanPhamDTO> UpdateSanPhamAsync(int id, SanPhamDTO sanPhamDTO, string imagePath);
        Task<bool> UpdateStockOnlyAsync(int sanPhamId, int newQty);


        Task<List<SanPham>?> GetTop10MostPurchasedProductsAsync();

    }
}
