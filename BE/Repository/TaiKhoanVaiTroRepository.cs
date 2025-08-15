using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using BE.DTOs;

namespace Repository
{
    public class TaiKhoanVaiTroRepository : ITaiKhoanVaiTroRepository
    {
        private readonly MyDbContext _context;

        public TaiKhoanVaiTroRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task CreateTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro)
        {
            _context.TaiKhoan_VaiTro.Add(taiKhoanVaiTro);
            await _context.SaveChangesAsync();
        }

        public Task DeleteTaiKhoanVaiTroAsync(int idTaiKhoan, int idVaiTro)
        {
            var taiKhoanVaiTro = _context.TaiKhoan_VaiTro.FirstOrDefaultAsync(t => t.ID_Tai_Khoan == idTaiKhoan && t.ID_Vai_Tro == idVaiTro);
            if (taiKhoanVaiTro != null)
            {
                _context.TaiKhoan_VaiTro.Remove(taiKhoanVaiTro.Result);
                return _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<TaiKhoanVaiTroDTO>> GetAllTaiKhoanVaiTroAsync()
        {
            return await _context.TaiKhoan_VaiTro.Include(t => t.VaiTro)
                                                  .Include(t => t.TaiKhoan)
                                                  .Where(t => t.TaiKhoan.ID_Nhan_Vien != null)
                                                  .Select(t => new TaiKhoanVaiTroDTO
                                                  {
                                                      ID_Tai_Khoan = t.ID_Tai_Khoan,
                                                      ID_Vai_Tro = t.ID_Vai_Tro,
                                                      TenVaiTro = t.VaiTro.Ten_Vai_Tro,
                                                      TenTaiKhoan = t.TaiKhoan.Ten_Nguoi_Dung,
                                                      Email = t.TaiKhoan.Email,
                                                      TrangThai = t.TaiKhoan.Trang_Thai
                                                  }).ToListAsync();
        }

        public async Task<TaiKhoanVaiTroDTO?> GetTaiKhoanVaiTroByIdAsync(int idTaiKhoan, int idVaiTro)
        {
            return await _context.TaiKhoan_VaiTro
                .Include(t => t.VaiTro)
                .Include(t => t.TaiKhoan)
                .Where(t => t.ID_Tai_Khoan == idTaiKhoan && t.ID_Vai_Tro == idVaiTro && t.TaiKhoan.ID_Nhan_Vien != null)
                .Select(t => new TaiKhoanVaiTroDTO
                {
                    ID_Tai_Khoan = t.ID_Tai_Khoan,
                    ID_Vai_Tro = t.ID_Vai_Tro,
                    TenVaiTro = t.VaiTro.Ten_Vai_Tro,
                    TenTaiKhoan = t.TaiKhoan.Ten_Nguoi_Dung,
                    Email = t.TaiKhoan.Email,
                    TrangThai = t.TaiKhoan.Trang_Thai
                }).FirstOrDefaultAsync();
        }

        public async Task UpdateTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro)
        {
            _context.TaiKhoan_VaiTro.Update(taiKhoanVaiTro);
            await _context.SaveChangesAsync();
        }
    }
}
