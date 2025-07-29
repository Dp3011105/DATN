using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class SizeRepository : ISizeRepository
    {
        private readonly MyDbContext _context;

        public SizeRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Size>> GetAllAsync()
        {
            return await _context.Size.ToListAsync();
        }

        public async Task<Size?> GetByIdAsync(int id)
        {
            return await _context.Size.FindAsync(id);
        }

        public async Task AddAsync(Size entity)
        {
            await _context.Size.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Size entity)
        {
            _context.Size.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Size.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}