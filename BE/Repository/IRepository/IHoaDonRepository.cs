using BE.models;

public interface IHoaDonRepository
{
    Task<IEnumerable<HoaDonDTO>> GetAllAsync();
    Task<HoaDon?> GetByIdAsync(int id);
    Task AddAsync(HoaDon entity);
    Task UpdateAsync(HoaDon entity);
    Task DeleteAsync(int id);
}
