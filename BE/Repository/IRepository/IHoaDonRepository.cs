namespace BE.Repository.IRepository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BE.DTOs;
    using BE.models;

    public interface IHoaDonRepository
    {
        Task<IEnumerable<HoaDonDTO>> GetAllAsync();
        Task<HoaDon> GetByIdAsync(int id);
        Task AddAsync(HoaDon entity);
        Task UpdateAsync(int id, HoaDon entity);
        Task DeleteAsync(int id);
    }
}
