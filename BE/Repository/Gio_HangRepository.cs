using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class Gio_HangRepository : IGio_HangRepository  // cái này dùng cho chức năng lấy danh sách dữ liệu giỏ hàng của khách hàng (Phước)
    {
        private readonly MyDbContext _context;

        public Gio_HangRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Gio_Hang> GetGioHangByKhachHangIdAsync(int idKhachHang)
        {
            return await _context.Gio_Hang
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ct => ct.San_Pham)
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ct => ct.Size)
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ct => ct.DoNgot)
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ct => ct.LuongDa)
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ct => ct.GioHangChiTiet_Toppings)
                        .ThenInclude(t => t.Topping)
                .FirstOrDefaultAsync(gh => gh.ID_Khach_Hang == idKhachHang && gh.Trang_Thai);
        }

        public async Task CreateGioHangAsync(Gio_Hang gioHang)
        {
            _context.Gio_Hang.Add(gioHang);
            await _context.SaveChangesAsync();
        }



        public async Task AddGioHangChiTietAsync(GioHang_ChiTiet gioHangChiTiet)
        {
            _context.GioHang_ChiTiet.Add(gioHangChiTiet);
            await _context.SaveChangesAsync();
        }



        public async Task<bool> DeleteGioHangChiTietAsync(int idGioHangChiTiet, int idKhachHang)
        {
            var gioHangChiTiet = await _context.GioHang_ChiTiet
                .Include(ct => ct.GioHangChiTiet_Toppings)
                .FirstOrDefaultAsync(ct => ct.ID_GioHang_ChiTiet == idGioHangChiTiet && ct.Gio_Hang.ID_Khach_Hang == idKhachHang);

            if (gioHangChiTiet == null)
            {
                return false; // Không tìm thấy chi tiết giỏ hàng hoặc không thuộc khách hàng
            }

            // Xóa các topping liên quan
            _context.GioHangChiTiet_Topping.RemoveRange(gioHangChiTiet.GioHangChiTiet_Toppings);

            // Xóa chi tiết giỏ hàng
            _context.GioHang_ChiTiet.Remove(gioHangChiTiet);

            await _context.SaveChangesAsync();
            return true;
        }



    }
}