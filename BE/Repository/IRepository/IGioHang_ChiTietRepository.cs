using BE.models;

namespace Repository.IRepository
{
    public interface IGioHang_ChiTietRepository
    {
        Task<IEnumerable<GioHang_ChiTiet>> GetAllAsync();
        Task<GioHang_ChiTiet?> GetByIdAsync(int id);
        Task AddAsync(GioHang_ChiTiet entity);
        Task UpdateAsync(GioHang_ChiTiet entity);
        Task DeleteAsync(int id);
    }
}