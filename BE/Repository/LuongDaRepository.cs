using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class LuongDaRepository : ILuongDaRepository
    {
        private readonly MyDbContext _context;

        public LuongDaRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LuongDa>> GetAllAsync()
        {
            return await _context.LuongDa.ToListAsync();
        }

        public async Task<LuongDa?> GetByIdAsync(int id)
        {
            return await _context.LuongDa.FindAsync(id);
        }

        public async Task AddAsync(LuongDa entity)
        {
            await _context.LuongDa.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LuongDa entity)
        {
            _context.LuongDa.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.LuongDa.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}