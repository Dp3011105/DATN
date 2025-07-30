using BE.models;

namespace Repository.IRepository
{
    public interface IGioHangChiTiet_ToppingRepository
    {
        Task<IEnumerable<GioHangChiTiet_Topping>> GetAllAsync();
        Task<GioHangChiTiet_Topping?> GetByIdAsync(int id);
        Task AddAsync(GioHangChiTiet_Topping entity);
        Task UpdateAsync(GioHangChiTiet_Topping entity);
        Task DeleteAsync(int id);
    }
}