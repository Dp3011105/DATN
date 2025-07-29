using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class ThueRepository : IThueRepository
    {
        private readonly MyDbContext _context;

        public ThueRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Thue>> GetAllAsync()
        {
            return await _context.Thue.ToListAsync();
        }

        public async Task<Thue?> GetByIdAsync(int id)
        {
            return await _context.Thue.FindAsync(id);
        }

        public async Task AddAsync(Thue entity)
        {
            await _context.Thue.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Thue entity)
        {
            _context.Thue.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Thue.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}