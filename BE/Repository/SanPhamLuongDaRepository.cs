using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class SanPhamLuongDaRepository : ISanPhamLuongDaRepository
    {
        private readonly MyDbContext _context;

        public SanPhamLuongDaRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPhamLuongDa>> GetAllAsync()
        {
            return await _context.SanPhamLuongDa.ToListAsync();
        }

        public async Task<SanPhamLuongDa?> GetByIdAsync(int id)
        {
            return await _context.SanPhamLuongDa.FindAsync(id);
        }

        public async Task AddAsync(SanPhamLuongDa entity)
        {
            await _context.SanPhamLuongDa.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SanPhamLuongDa entity)
        {
            _context.SanPhamLuongDa.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.SanPhamLuongDa.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}