using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class HinhThucThanhToanRepository : IHinhThucThanhToanRepository
    {
        private readonly MyDbContext _context;

        public HinhThucThanhToanRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HinhThucThanhToan>> GetAllAsync()
        {
            return await _context.Hinh_Thuc_Thanh_Toan.ToListAsync();
        }

        public async Task<HinhThucThanhToan?> GetByIdAsync(int id)
        {
            return await _context.Hinh_Thuc_Thanh_Toan.FindAsync(id);
        }

        public async Task AddAsync(HinhThucThanhToan entity)
        {
            await _context.Hinh_Thuc_Thanh_Toan.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HinhThucThanhToan entity)
        {
            _context.Hinh_Thuc_Thanh_Toan.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Hinh_Thuc_Thanh_Toan.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}