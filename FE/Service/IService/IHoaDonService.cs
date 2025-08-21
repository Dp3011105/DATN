using BE.models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IHoaDonService
{
    Task<IEnumerable<HoaDon>> GetAllAsync();
    Task<HoaDon> GetByIdAsync(int id);
    Task AddAsync(HoaDon entity);
    Task UpdateAsync(int id, HoaDon entity);
    Task DeleteAsync(int id);

    // mới: cập nhật trạng thái nhẹ
    Task<bool> UpdateTrangThaiAsync(int hoaDonId, string trangThaiDb, string? lyDoHuy);
}
