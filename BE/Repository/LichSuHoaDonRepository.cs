using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class LichSuHoaDonRepository : ILichSuHoaDonRepository
    {
        private readonly MyDbContext _context;

        public LichSuHoaDonRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LichSuHoaDon>> GetAllAsync()
        {
            return await _context.Lich_Su_Hoa_Don.ToListAsync();
        }

        public async Task<LichSuHoaDon?> GetByIdAsync(int id)
        {
            return await _context.Lich_Su_Hoa_Don.FindAsync(id);
        }

        public async Task AddAsync(LichSuHoaDon entity)
        {
            await _context.Lich_Su_Hoa_Don.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LichSuHoaDon entity)
        {
            _context.Lich_Su_Hoa_Don.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Lich_Su_Hoa_Don.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}