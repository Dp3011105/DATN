using BE.Data;
using BE.models;
using Microsoft.EntityFrameworkCore;

public class HoaDonRepository : IHoaDonRepository
{
    private readonly MyDbContext _context;
    public HoaDonRepository(MyDbContext context) => _context = context;

    public async Task<IEnumerable<HoaDonDTO>> GetAllAsync()
    {
        return await _context.Hoa_Don.Select(h => new HoaDonDTO
        {
            ID_Hoa_Don = h.ID_Hoa_Don,
            Ma_Hoa_Don = h.Ma_Hoa_Don,
            Ngay_Tao = h.Ngay_Tao,
            Tong_Tien = h.Tong_Tien,
            Trang_Thai = h.Trang_Thai
        }).ToListAsync();
    }

    public async Task<HoaDon?> GetByIdAsync(int id)
        => await _context.Hoa_Don.FindAsync(id);

    public async Task AddAsync(HoaDon entity)
    {
        await _context.Hoa_Don.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(HoaDon entity)
    {
        _context.Hoa_Don.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Hoa_Don.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
