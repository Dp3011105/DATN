using System.Collections.Generic;
using System.Threading.Tasks;
using BE.models;

namespace Service.IService
{
    public interface IHoaDonChiTietToppingService
    {
        Task<IEnumerable<HoaDonChiTietTopping>> GetAllAsync();
        Task<HoaDonChiTietTopping?> GetByIdAsync(int id);
        Task AddAsync(HoaDonChiTietTopping entity);
        Task UpdateAsync(HoaDonChiTietTopping entity);
        Task DeleteAsync(int id);
    }
}