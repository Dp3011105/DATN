using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class HoaDonChiTietToppingRepository : IHoaDonChiTietToppingRepository
    {
        private readonly MyDbContext _context;

        public HoaDonChiTietToppingRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HoaDonChiTietTopping>> GetAllAsync()
        {
            return await _context.HoaDonChiTiet_Topping.ToListAsync();
        }

        public async Task<HoaDonChiTietTopping?> GetByIdAsync(int id)
        {
            return await _context.HoaDonChiTiet_Topping.FindAsync(id);
        }

        public async Task AddAsync(HoaDonChiTietTopping entity)
        {
            await _context.HoaDonChiTiet_Topping.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HoaDonChiTietTopping entity)
        {
            _context.HoaDonChiTiet_Topping.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.HoaDonChiTiet_Topping.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}