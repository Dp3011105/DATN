using FE.Models;
namespace FE.Service.IService
{
    public interface IProductDetailsService
    {
        // DoNgot
        Task<List<DoNgot>> GetAllDoNgotsAsync();
        Task<DoNgot> GetDoNgotByIdAsync(int id);
        Task CreateDoNgotAsync(DoNgot doNgot);
        Task UpdateDoNgotAsync(DoNgot doNgot);

        // Size
        Task<List<Size>> GetAllSizesAsync();
        Task<Size> GetSizeByIdAsync(int id);
        Task CreateSizeAsync(Size size);
        Task UpdateSizeAsync(Size size);

        // Topping
        Task<List<Topping>> GetAllToppingsAsync();
        Task<Topping> GetToppingByIdAsync(int id);
        Task CreateToppingAsync(Topping topping);
        Task UpdateToppingAsync(Topping topping);

        // LuongDa
        Task<List<LuongDa>> GetAllLuongDasAsync();
        Task<LuongDa> GetLuongDaByIdAsync(int id);
        Task CreateLuongDaAsync(LuongDa luongDa);
        Task UpdateLuongDaAsync(LuongDa luongDa);
    }
}
