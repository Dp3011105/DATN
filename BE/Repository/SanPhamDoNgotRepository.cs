using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class SanPhamDoNgotRepository : ISanPhamDoNgotRepository
    {
        private readonly MyDbContext _context;

        public SanPhamDoNgotRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPhamDoNgot>> GetAllAsync()
        {
            return await _context.SanPham_DoNgot.ToListAsync();
        }

        public async Task<SanPhamDoNgot?> GetByIdAsync(int id)
        {
            return await _context.SanPham_DoNgot.FindAsync(id);
        }

        public async Task AddAsync(SanPhamDoNgot entity)
        {
            await _context.SanPham_DoNgot.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SanPhamDoNgot entity)
        {
            _context.SanPham_DoNgot.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.SanPham_DoNgot.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}