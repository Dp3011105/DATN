using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class HoaDonChiTietRepository : IHoaDonChiTietRepository
    {
        private readonly MyDbContext _context;

        public HoaDonChiTietRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HoaDonChiTiet>> GetAllAsync()
        {
            return await _context.HoaDon_ChiTiet.ToListAsync();
        }

        public async Task<HoaDonChiTiet?> GetByIdAsync(int id)
        {
            return await _context.HoaDon_ChiTiet.FindAsync(id);
        }

        public async Task AddAsync(HoaDonChiTiet entity)
        {
            await _context.HoaDon_ChiTiet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HoaDonChiTiet entity)
        {
            _context.HoaDon_ChiTiet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.HoaDon_ChiTiet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}