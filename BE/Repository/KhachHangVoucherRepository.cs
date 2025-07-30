using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class KhachHangVoucherRepository : IKhachHangVoucherRepository
    {
        private readonly MyDbContext _context;

        public KhachHangVoucherRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhachHangVoucher>> GetAllAsync()
        {
            return await _context.KhachHang_Voucher.ToListAsync();
        }

        public async Task<KhachHangVoucher?> GetByIdAsync(int id)
        {
            return await _context.KhachHang_Voucher.FindAsync(id);
        }

        public async Task AddAsync(KhachHangVoucher entity)
        {
            await _context.KhachHang_Voucher.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhachHangVoucher entity)
        {
            _context.KhachHang_Voucher.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.KhachHang_Voucher.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}