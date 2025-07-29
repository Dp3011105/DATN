using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class GioHangChiTiet_ToppingRepository : IGioHangChiTiet_ToppingRepository
    {
        private readonly MyDbContext _context;

        public GioHangChiTiet_ToppingRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GioHangChiTiet_Topping>> GetAllAsync()
        {
            return await _context.GioHangChiTiet_Topping.ToListAsync();
        }

        public async Task<GioHangChiTiet_Topping?> GetByIdAsync(int id)
        {
            return await _context.GioHangChiTiet_Topping.FindAsync(id);
        }

        public async Task AddAsync(GioHangChiTiet_Topping entity)
        {
            await _context.GioHangChiTiet_Topping.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GioHangChiTiet_Topping entity)
        {
            _context.GioHangChiTiet_Topping.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.GioHangChiTiet_Topping.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}