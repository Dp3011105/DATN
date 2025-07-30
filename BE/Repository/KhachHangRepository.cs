using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly MyDbContext _context;

        public KhachHangRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhachHang>> GetAllAsync()
        {
            return await _context.Khach_Hang.ToListAsync();
        }

        public async Task<KhachHang?> GetByIdAsync(int id)
        {
            return await _context.Khach_Hang.FindAsync(id);
        }

        public async Task AddAsync(KhachHang entity)
        {
            await _context.Khach_Hang.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhachHang entity)
        {
            _context.Khach_Hang.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Khach_Hang.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}