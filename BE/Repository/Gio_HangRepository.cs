using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class Gio_HangRepository : IGio_HangRepository
    {
        private readonly MyDbContext _context;

        public Gio_HangRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gio_Hang>> GetAllAsync()
        {
            return await _context.Gio_Hang.ToListAsync();
        }

        public async Task<Gio_Hang?> GetByIdAsync(int id)
        {
            return await _context.Gio_Hang.FindAsync(id);
        }

        public async Task AddAsync(Gio_Hang entity)
        {
            await _context.Gio_Hang.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Gio_Hang entity)
        {
            _context.Gio_Hang.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Gio_Hang.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}