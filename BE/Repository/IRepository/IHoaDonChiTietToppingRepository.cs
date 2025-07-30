using BE.models;

namespace Repository.IRepository
{
    public interface IHoaDonChiTietToppingRepository
    {
        Task<IEnumerable<HoaDonChiTietTopping>> GetAllAsync();
        Task<HoaDonChiTietTopping?> GetByIdAsync(int id);
        Task AddAsync(HoaDonChiTietTopping entity);
        Task UpdateAsync(HoaDonChiTietTopping entity);
        Task DeleteAsync(int id);
    }
}