using FE.Models;

namespace FE.Service.IService
{
    public interface IProductService
    {
        //Task<List<SanPham>> GetAllProductsAsync();
        //Task<SanPham> GetProductByIdAsync(int id);
        //Task<List<Size>> GetSizesAsync();
        //Task<List<Topping>> GetToppingsAsync();
        //Task<List<LuongDa>> GetLuongDasAsync();
        //Task<List<DoNgot>> GetDoNgotsAsync();
        //Task<string> UploadImageAsync(IFormFile image);
        //Task<bool> AddProductAsync(AddProductViewModel model);
        //Task<bool> UpdateProductAsync(UpdateProductViewModel model);


        Task<List<SanPham>> GetAllProductsAsync();
        Task<SanPham> GetProductByIdAsync(int id);
        Task<List<Size>> GetSizesAsync();
        Task<List<Topping>> GetToppingsAsync();
        Task<List<LuongDa>> GetLuongDasAsync();
        Task<List<DoNgot>> GetDoNgotsAsync();
        Task<bool> AddProductAsync(AddProductViewModel model);
        Task<bool> UpdateProductAsync(UpdateProductViewModel model);



        Task<List<SanPham>> GetMostPurchasedProductsAsync(); // Method mới cho API 10 sản phẩm phổ biến

    }
}
