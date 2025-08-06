using BE.models;

public interface IHoaDonService
{
    Task<HoaDon?> GetByIdAsync(int id);
    Task AddAsync(HoaDon entity);
    Task UpdateAsync(HoaDon entity);
    Task DeleteAsync(int id);
    Task<IEnumerable<HoaDonDTO>> GetAllAsync();

}
