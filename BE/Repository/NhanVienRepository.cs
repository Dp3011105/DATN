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

        public async Task AddAsync(NhanVien entity)
        {
            try
            {
                _context.Nhan_Vien.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                entity.Trang_Thai = false; 
                _context.Nhan_Vien.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while deleting the entity.", ex);
            }
        }

        public async Task<IEnumerable<NhanVien>> GetAllAsync()
        {
            return await _context.Nhan_Vien.ToListAsync();
        }

        public async Task<NhanVien> GetByIdAsync(int id)
        {
            return await _context.Nhan_Vien
                .FirstOrDefaultAsync(nv => nv.ID_Nhan_Vien == id) 
                ?? throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        public async Task UpdateAsync(int id, NhanVien entity)
        {
            try
            {
                var existingEntity = await GetByIdAsync(id);
                if (existingEntity == null)
                {
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                existingEntity.Ho_Ten = entity.Ho_Ten;
                existingEntity.Email = entity.Email;
                existingEntity.So_Dien_Thoai = entity.So_Dien_Thoai;
                existingEntity.Dia_Chi = entity.Dia_Chi;
                existingEntity.NamSinh = entity.NamSinh;
                existingEntity.GioiTinh = entity.GioiTinh;
                existingEntity.CCCD = entity.CCCD;
                existingEntity.Ghi_Chu = entity.Ghi_Chu;
                existingEntity.Trang_Thai = entity.Trang_Thai;
                existingEntity.AnhNhanVien = entity.AnhNhanVien;
                _context.Nhan_Vien.Update(existingEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while updating the entity.", ex);
            }
        }
    }
}