using BE.models;

public interface IHoaDonService
{

    public interface IHoaDonService
    {
        Task<IEnumerable<HoaDon>> GetAllAsync();
        Task<HoaDon> GetByIdAsync(int id);

        Task AddAsync(HoaDon entity);
        Task UpdateAsync(int id,HoaDon entity);
        Task DeleteAsync(int id);
    }
}

