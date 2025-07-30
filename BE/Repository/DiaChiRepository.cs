using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class DiaChiRepository : IDiaChiRepository
    {
        private readonly MyDbContext _context;

        public DiaChiRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiaChi>> GetAllAsync()
        {
            return await _context.Dia_Chi.ToListAsync();
        }

        public async Task<DiaChi?> GetByIdAsync(int id)
        {
            return await _context.Dia_Chi.FindAsync(id);
        }

        public async Task AddAsync(DiaChi entity)
        {
            await _context.Dia_Chi.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiaChi entity)
        {
            _context.Dia_Chi.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Dia_Chi.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}