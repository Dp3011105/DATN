using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class SanPhamToppingRepository : ISanPhamToppingRepository
    {
        private readonly MyDbContext _context;

        public SanPhamToppingRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPhamTopping>> GetAllAsync()
        {
            return await _context.SanPham_Topping.ToListAsync();
        }

        public async Task<SanPhamTopping?> GetByIdAsync(int id)
        {
            return await _context.SanPham_Topping.FindAsync(id);
        }

        public async Task AddAsync(SanPhamTopping entity)
        {
            await _context.SanPham_Topping.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SanPhamTopping entity)
        {
            _context.SanPham_Topping.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.SanPham_Topping.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}