using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class KhachHangDiaChiRepository : IKhachHangDiaChiRepository
    {
        private readonly MyDbContext _context;

        public KhachHangDiaChiRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhachHangDiaChi>> GetAllAsync()
        {
            return await _context.KhachHang_DiaChi.ToListAsync();
        }

        public async Task<KhachHangDiaChi?> GetByIdAsync(int id)
        {
            return await _context.KhachHang_DiaChi.FindAsync(id);
        }

        public async Task AddAsync(KhachHangDiaChi entity)
        {
            await _context.KhachHang_DiaChi.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhachHangDiaChi entity)
        {
            _context.KhachHang_DiaChi.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.KhachHang_DiaChi.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}