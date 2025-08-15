using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly MyDbContext _context;

        public TaiKhoanRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaiKhoan entity)
        {
            try
            {
                _context.Tai_Khoan.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
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
                    throw new KeyNotFoundException("Entity not found.");
                }
                _context.Tai_Khoan.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while deleting the entity.", ex);
            }
        }

        public async Task<IEnumerable<TaiKhoan>> GetAllAsync()
        {
            return await _context.Tai_Khoan
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TaiKhoan> GetByIdAsync(int id)
        {
            return await _context.Tai_Khoan
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ID_Tai_Khoan == id);
        }

        public async Task UpdateAsync(int id, TaiKhoan entity)
        {
            try
            {
                var existingEntity = await GetByIdAsync(id);
                if (existingEntity == null)
                {
                    throw new KeyNotFoundException("Entity not found.");
                }
                existingEntity.Ten_Nguoi_Dung = entity.Ten_Nguoi_Dung;
                existingEntity.Mat_Khau = entity.Mat_Khau;
                existingEntity.Email = entity.Email;
                existingEntity.ID_Nhan_Vien = entity.ID_Nhan_Vien;
                existingEntity.ID_Khach_Hang = entity.ID_Khach_Hang;
                existingEntity.Ngay_Cap_Nhat = DateTime.Now;
                _context.Tai_Khoan.Update(existingEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while updating the entity.", ex);
            }
        }
    }
}