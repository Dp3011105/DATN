using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IGioHangChiTiet_ToppingService
    {
        Task<IEnumerable<GioHangChiTiet_Topping>> GetAllAsync();
        Task<GioHangChiTiet_Topping?> GetByIdAsync(int id);
        Task AddAsync(GioHangChiTiet_Topping entity);
        Task UpdateAsync(GioHangChiTiet_Topping entity);
        Task DeleteAsync(int id);
    }
}