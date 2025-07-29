using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class SanPhamSizeRepository : ISanPhamSizeRepository
    {
        private readonly MyDbContext _context;

        public SanPhamSizeRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPhamSize>> GetAllAsync()
        {
            return await _context.SanPham_Size.ToListAsync();
        }

        public async Task<SanPhamSize?> GetByIdAsync(int id)
        {
            return await _context.SanPham_Size.FindAsync(id);
        }

        public async Task AddAsync(SanPhamSize entity)
        {
            await _context.SanPham_Size.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SanPhamSize entity)
        {
            _context.SanPham_Size.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.SanPham_Size.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}