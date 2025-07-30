using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly MyDbContext _context;

        public HoaDonRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HoaDon>> GetAllAsync()
        {
            return await _context.Hoa_Don.ToListAsync();
        }

        public async Task<HoaDon?> GetByIdAsync(int id)
        {
            return await _context.Hoa_Don.FindAsync(id);
        }

        public async Task AddAsync(HoaDon entity)
        {
            await _context.Hoa_Don.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HoaDon entity)
        {
            _context.Hoa_Don.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Hoa_Don.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}