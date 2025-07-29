using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly MyDbContext _context;

        public VoucherRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Voucher>> GetAllAsync()
        {
            return await _context.Voucher.ToListAsync();
        }

        public async Task<Voucher?> GetByIdAsync(int id)
        {
            return await _context.Voucher.FindAsync(id);
        }

        public async Task AddAsync(Voucher entity)
        {
            await _context.Voucher.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Voucher entity)
        {
            _context.Voucher.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Voucher.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}