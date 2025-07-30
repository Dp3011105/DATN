using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class TaiKhoanVaiTroRepository : ITaiKhoanVaiTroRepository
    {
        private readonly MyDbContext _context;

        public TaiKhoanVaiTroRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaiKhoanVaiTro>> GetAllAsync()
        {
            return await _context.TaiKhoan_VaiTro.ToListAsync();
        }

        public async Task<TaiKhoanVaiTro?> GetByIdAsync(int id)
        {
            return await _context.TaiKhoan_VaiTro.FindAsync(id);
        }

        public async Task AddAsync(TaiKhoanVaiTro entity)
        {
            await _context.TaiKhoan_VaiTro.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaiKhoanVaiTro entity)
        {
            _context.TaiKhoan_VaiTro.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.TaiKhoan_VaiTro.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}