using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class DiemDanhRepository : IDiemDanhRepository
    {
        private readonly MyDbContext _context;

        public DiemDanhRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiemDanh>> GetAllAsync()
        {
            return await _context.Diem_Danh.ToListAsync();
        }

        public async Task<DiemDanh?> GetByIdAsync(int id)
        {
            return await _context.Diem_Danh.FindAsync(id);
        }

        public async Task AddAsync(DiemDanh entity)
        {
            await _context.Diem_Danh.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiemDanh entity)
        {
            _context.Diem_Danh.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Diem_Danh.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}