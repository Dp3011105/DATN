using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly MyDbContext _context;

        public TaiKhoanRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaiKhoan>> GetAllAsync()
        {
            return await _context.Tai_Khoan.ToListAsync();
        }

        public async Task<TaiKhoan?> GetByIdAsync(int id)
        {
            return await _context.Tai_Khoan.FindAsync(id);
        }

        public async Task AddAsync(TaiKhoan entity)
        {
            await _context.Tai_Khoan.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaiKhoan entity)
        {
            _context.Tai_Khoan.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Tai_Khoan.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}