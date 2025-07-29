using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class GioHang_ChiTietRepository : IGioHang_ChiTietRepository
    {
        private readonly MyDbContext _context;

        public GioHang_ChiTietRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GioHang_ChiTiet>> GetAllAsync()
        {
            return await _context.GioHang_ChiTiet.ToListAsync();
        }

        public async Task<GioHang_ChiTiet?> GetByIdAsync(int id)
        {
            return await _context.GioHang_ChiTiet.FindAsync(id);
        }

        public async Task AddAsync(GioHang_ChiTiet entity)
        {
            await _context.GioHang_ChiTiet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GioHang_ChiTiet entity)
        {
            _context.GioHang_ChiTiet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.GioHang_ChiTiet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}