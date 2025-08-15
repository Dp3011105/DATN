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

        public async Task AddAsync(VaiTro entity)
        {
            try
            {
                _context.Vai_Tro.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while adding the role.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var vaiTro = await GetByIdAsync(id);
                if (vaiTro == null)
                {
                    throw new KeyNotFoundException("Role not found.");
                }
                _context.Vai_Tro.Remove(vaiTro);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while deleting the role.", ex);
            }
        }

        public async Task<IEnumerable<VaiTro>> GetAllAsync()
        {
            return await _context.Vai_Tro.ToListAsync();
        }

        public async Task<VaiTro> GetByIdAsync(int id)
        {
            return await _context.Vai_Tro
                .FirstOrDefaultAsync(v => v.ID_Vai_Tro == id) 
                ?? throw new KeyNotFoundException("Role not found.");
        }

        public async Task UpdateAsync(int id, VaiTro entity)
        {
            try
            {
                var vaiTro = await GetByIdAsync(id);
                if (vaiTro == null)
                {
                    throw new KeyNotFoundException("Role not found.");
                }
                vaiTro.Ten_Vai_Tro = entity.Ten_Vai_Tro;
                _context.Vai_Tro.Update(vaiTro);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while updating the role.", ex);
            }
        }
    }
}