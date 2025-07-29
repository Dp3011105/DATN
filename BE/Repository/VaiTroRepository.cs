using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class VaiTroRepository : IVaiTroRepository
    {
        private readonly MyDbContext _context;

        public VaiTroRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VaiTro>> GetAllAsync()
        {
            return await _context.Vai_Tro.ToListAsync();
        }

        public async Task<VaiTro?> GetByIdAsync(int id)
        {
            return await _context.Vai_Tro.FindAsync(id);
        }

        public async Task AddAsync(VaiTro entity)
        {
            await _context.Vai_Tro.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VaiTro entity)
        {
            _context.Vai_Tro.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Vai_Tro.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}