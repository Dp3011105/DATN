using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly MyDbContext _context;

        public SanPhamRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _context.San_Pham.ToListAsync();
        }

        public async Task<SanPham?> GetByIdAsync(int id)
        {
            return await _context.San_Pham.FindAsync(id);
        }

        public async Task AddAsync(SanPham entity)
        {
            await _context.San_Pham.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SanPham entity)
        {
            _context.San_Pham.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.San_Pham.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}