//using BE.models;
//using BE.Data;
//using Microsoft.EntityFrameworkCore;
//using Repository.IRepository;

//namespace Repository
//{
//    public class TaiKhoanRepository : ITaiKhoanRepository// Hưng Repository cho tài khoản người dùng
//    {
//        private readonly MyDbContext _context;

//        public TaiKhoanRepository(MyDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<TaiKhoan> AddAsync(TaiKhoan entity)
//        {
//            try
//            {
//                _context.Tai_Khoan.Add(entity);
//                await _context.SaveChangesAsync();
//                return entity; 
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("An error occurred while adding the entity.", ex);
//            }
//        }

//        public async Task DeleteAsync(int id)
//        {
//            try
//            {
//                var entity = await GetByIdAsync(id);
//                if (entity == null)
//                {
//                    throw new KeyNotFoundException("Entity not found.");
//                }
//                _context.Tai_Khoan.Remove(entity);
//                await _context.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                // Log the exception
//                throw new Exception("An error occurred while deleting the entity.", ex);
//            }
//        }

//        public async Task<IEnumerable<TaiKhoan>> GetAllAsync()
//        {
//            return await _context.Tai_Khoan
//                               .Include(t => t.NhanVien) // Join bảng NhanVien
//                               .Include(t => t.TaiKhoanVaiTros).ThenInclude(v => v.VaiTro)
//                               .ToListAsync();
//        }

//        public async Task<TaiKhoan> GetByIdAsync(int id)
//        {
//            return await _context.Tai_Khoan
//                                 .Include(t => t.NhanVien)
//                                 .FirstOrDefaultAsync(t => t.ID_Tai_Khoan == id);
//        }

//        public async Task UpdateAsync(int id, TaiKhoan entity)
//        {
//            try
//            {
//                var existingEntity = await GetByIdAsync(id);
//                if (existingEntity == null)
//                {
//                    throw new KeyNotFoundException("Entity not found.");
//                }
//                existingEntity.Ten_Nguoi_Dung = entity.Ten_Nguoi_Dung;
//                existingEntity.Mat_Khau = entity.Mat_Khau;
//                existingEntity.Email = entity.Email;
//                existingEntity.ID_Nhan_Vien = entity.ID_Nhan_Vien;
//                existingEntity.ID_Khach_Hang = entity.ID_Khach_Hang;
//                existingEntity.Ngay_Cap_Nhat = DateTime.Now;
//                _context.Tai_Khoan.Update(existingEntity);
//                await _context.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                // Log the exception
//                throw new Exception("An error occurred while updating the entity.", ex);
//            }
//        }
//    }
//}

using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class TaiKhoanRepository : ITaiKhoanRepository// Hưng Repository cho tài khoản người dùng
    {
        private readonly MyDbContext _context;

        public TaiKhoanRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<TaiKhoan> AddAsync(TaiKhoan entity)
        {
            try
            {
                _context.Tai_Khoan.Add(entity);
                // Tự động gán vai trò 3 (nhân viên) nếu có ID_Nhan_Vien (tức là tài khoản cho nhân viên)
                if (entity.ID_Nhan_Vien.HasValue)
                {
                    entity.TaiKhoanVaiTros.Add(new TaiKhoanVaiTro { ID_Vai_Tro = 3 });
                }
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
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
                               .Include(t => t.NhanVien) // Join bảng NhanVien
                               .Include(t => t.TaiKhoanVaiTros).ThenInclude(v => v.VaiTro)
                               .ToListAsync();
        }

        public async Task<TaiKhoan> GetByIdAsync(int id)
        {
            return await _context.Tai_Khoan
                                 .Include(t => t.NhanVien)
                                 .FirstOrDefaultAsync(t => t.ID_Tai_Khoan == id);
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