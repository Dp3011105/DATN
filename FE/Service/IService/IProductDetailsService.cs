using FE.Models;
namespace FE.Service.IService
{
    public interface IProductDetailsService
    {
        // DoNgot
        Task<List<DoNgot>> GetDoNgotsAsync();
        Task<DoNgot> GetDoNgotByIdAsync(int id);
        Task<bool> AddDoNgotAsync(DoNgot doNgot);
        Task<bool> UpdateDoNgotAsync(DoNgot doNgot);

        // Size
        Task<List<Size>> GetSizesAsync();
        Task<Size> GetSizeByIdAsync(int id);
        Task<bool> AddSizeAsync(Size size);
        Task<bool> UpdateSizeAsync(Size size);

        // Topping
        Task<List<Topping>> GetToppingsAsync();
        Task<Topping> GetToppingByIdAsync(int id);
        Task<bool> AddToppingAsync(Topping topping);
        Task<bool> UpdateToppingAsync(Topping topping);

        // LuongDa
        Task<List<LuongDa>> GetLuongDasAsync();
        Task<LuongDa> GetLuongDaByIdAsync(int id);
        Task<bool> AddLuongDaAsync(LuongDa luongDa);
        Task<bool> UpdateLuongDaAsync(LuongDa luongDa);
    }
}

