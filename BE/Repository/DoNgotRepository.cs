using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class DoNgotRepository : IDoNgotRepository
    {
        private readonly MyDbContext _context;

        public DoNgotRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoNgot>> GetAllAsync()
        {
            return await _context.DoNgot.ToListAsync();
        }

        public async Task<DoNgot?> GetByIdAsync(int id)
        {
            return await _context.DoNgot.FindAsync(id);
        }

        public async Task AddAsync(DoNgot entity)
        {
            await _context.DoNgot.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DoNgot entity)
        {
            _context.DoNgot.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.DoNgot.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}