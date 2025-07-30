using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly MyDbContext _context;

        public NhanVienRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhanVien>> GetAllAsync()
        {
            return await _context.Nhan_Vien.ToListAsync();
        }

        public async Task<NhanVien?> GetByIdAsync(int id)
        {
            return await _context.Nhan_Vien.FindAsync(id);
        }

        public async Task AddAsync(NhanVien entity)
        {
            await _context.Nhan_Vien.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhanVien entity)
        {
            _context.Nhan_Vien.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Nhan_Vien.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}