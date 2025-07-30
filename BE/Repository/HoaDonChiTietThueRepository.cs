using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class HoaDonChiTietThueRepository : IHoaDonChiTietThueRepository
    {
        private readonly MyDbContext _context;

        public HoaDonChiTietThueRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HoaDonChiTietThue>> GetAllAsync()
        {
            return await _context.HoaDonChiTiet_Thue.ToListAsync();
        }

        public async Task<HoaDonChiTietThue?> GetByIdAsync(int id)
        {
            return await _context.HoaDonChiTiet_Thue.FindAsync(id);
        }

        public async Task AddAsync(HoaDonChiTietThue entity)
        {
            await _context.HoaDonChiTiet_Thue.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HoaDonChiTietThue entity)
        {
            _context.HoaDonChiTiet_Thue.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.HoaDonChiTiet_Thue.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}