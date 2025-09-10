using BE.Data;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class AccountRepository : IAccountRepository // dùng cho chức năng đăng ký (phước)                                               
    {
        private readonly MyDbContext _context;

        public AccountRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Tai_Khoan.AnyAsync(t => t.Ten_Nguoi_Dung == username);
        }

        public async Task AddTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            _context.Tai_Khoan.Add(taiKhoan);
        }

        public async Task AddTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro)
        {
            _context.TaiKhoan_VaiTro.Add(taiKhoanVaiTro);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<TaiKhoan> GetTaiKhoanByUsernameAsync(string username)
        {
            return await _context.Tai_Khoan
                .FirstOrDefaultAsync(t => t.Ten_Nguoi_Dung == username);
        }

        public async Task<List<int>> GetVaiTroIdsByTaiKhoanIdAsync(int taiKhoanId)
        {
            return await _context.TaiKhoan_VaiTro
                .Where(t => t.ID_Tai_Khoan == taiKhoanId)
                .Select(t => t.ID_Vai_Tro)
                .ToListAsync();
        }

        public async Task<TaiKhoan> GetTaiKhoanByKhachHangIdAsync(int idKhachHang)
        {
            return await _context.Tai_Khoan
                .FirstOrDefaultAsync(t => t.ID_Khach_Hang == idKhachHang);
        }

        public async Task UpdateTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            _context.Tai_Khoan.Update(taiKhoan);
        }



    }
}
