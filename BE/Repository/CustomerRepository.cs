using BE.Data;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class CustomerRepository : ICustomerRepository // dùng cho chức năng đăng ký (Phước)
    {
        private readonly MyDbContext _context;

        public CustomerRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Khach_Hang.AnyAsync(k => k.Email == email);
        }

        public async Task AddKhachHangAsync(KhachHang khachHang)
        {
            _context.Khach_Hang.Add(khachHang);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }



        public async Task<KhachHang> GetKhachHangByEmailAsync(string email)
        {
            return await _context.Khach_Hang.FirstOrDefaultAsync(k => k.Email == email);
        }
    }

}
