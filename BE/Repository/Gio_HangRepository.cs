using BE.Data;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class Gio_HangRepository : IGio_HangRepository
    {
        private readonly MyDbContext _context; // Thay YourDbContext bằng tên DbContext của bạn

        public Gio_HangRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Gio_Hang> GetCartByCustomerIdAsync(int idKhachHang)
        {
            return await _context.Gio_Hang
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ghct => ghct.San_Pham)
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ghct => ghct.Size)
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ghct => ghct.DoNgot)
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ghct => ghct.LuongDa)
                .Include(gh => gh.GioHang_ChiTiets)
                    .ThenInclude(ghct => ghct.GioHangChiTiet_Toppings)
                    .ThenInclude(ghctt => ghctt.Topping)
                .FirstOrDefaultAsync(gh => gh.ID_Khach_Hang == idKhachHang && gh.Trang_Thai);
        }

        public async Task<SanPhamKhuyenMai> GetActivePromotionForProductAsync(int idSanPham, DateTime currentDate)
        {
            return await _context.SanPhamKhuyenMai
                .Include(spkm => spkm.BangKhuyenMai)
                .Where(spkm => spkm.ID_San_Pham == idSanPham
                    && spkm.BangKhuyenMai.Trang_Thai
                    && spkm.BangKhuyenMai.Ngay_Bat_Dau <= currentDate
                    && spkm.BangKhuyenMai.Ngay_Ket_Thuc >= currentDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddCartAsync(Gio_Hang gioHang)
        {
            await _context.Gio_Hang.AddAsync(gioHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Gio_Hang gioHang)
        {
            _context.Gio_Hang.Update(gioHang);
            await _context.SaveChangesAsync();
        }

        public async Task AddCartDetailAsync(GioHang_ChiTiet gioHangChiTiet)
        {
            await _context.GioHang_ChiTiet.AddAsync(gioHangChiTiet);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCartDetailAsync(int idGioHangChiTiet)
        {
            var gioHangChiTiet = await _context.GioHang_ChiTiet
                .Include(ghct => ghct.GioHangChiTiet_Toppings)
                .FirstOrDefaultAsync(ghct => ghct.ID_GioHang_ChiTiet == idGioHangChiTiet);

            if (gioHangChiTiet == null)
            {
                return false;
            }

            _context.GioHangChiTiet_Topping.RemoveRange(gioHangChiTiet.GioHangChiTiet_Toppings);
            _context.GioHang_ChiTiet.Remove(gioHangChiTiet);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
