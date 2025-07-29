using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class HoaDonVoucherRepository : IHoaDonVoucherRepository
    {
        private readonly MyDbContext _context;

        public HoaDonVoucherRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HoaDonVoucher>> GetAllAsync()
        {
            return await _context.HoaDonVouchers.ToListAsync();
        }

        public async Task<HoaDonVoucher?> GetByIdAsync(int id)
        {
            return await _context.HoaDonVouchers.FindAsync(id);
        }

        public async Task AddAsync(HoaDonVoucher entity)
        {
            await _context.HoaDonVouchers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HoaDonVoucher entity)
        {
            _context.HoaDonVouchers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.HoaDonVouchers.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}