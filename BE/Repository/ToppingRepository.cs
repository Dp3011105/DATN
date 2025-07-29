using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class ToppingRepository : IToppingRepository
    {
        private readonly MyDbContext _context;

        public ToppingRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Topping>> GetAllAsync()
        {
            return await _context.Topping.ToListAsync();
        }

        public async Task<Topping?> GetByIdAsync(int id)
        {
            return await _context.Topping.FindAsync(id);
        }

        public async Task AddAsync(Topping entity)
        {
            await _context.Topping.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Topping entity)
        {
            _context.Topping.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Topping.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}