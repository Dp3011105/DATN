using BE.Data;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly MyDbContext _context;

        public ProfileRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<KhachHang> GetKhachHangByIdAsync(int khachHangId)
        {
            return await _context.Khach_Hang
                .FirstOrDefaultAsync(k => k.ID_Khach_Hang == khachHangId);
        }

        // Thêm method kiểm tra email đã tồn tại
        public async Task<KhachHang> GetKhachHangByEmailAsync(string email)
        {
            return await _context.Khach_Hang
                .FirstOrDefaultAsync(k => k.Email == email);
        }

        public async Task<bool> UpdateKhachHangAsync(KhachHang khachHang)
        {
            try
            {
                // Find existing entity and update its properties
                var existingKhachHang = await _context.Khach_Hang
                    .FirstOrDefaultAsync(k => k.ID_Khach_Hang == khachHang.ID_Khach_Hang);

                if (existingKhachHang != null)
                {
                    // Update properties explicitly including email
                    existingKhachHang.Ho_Ten = khachHang.Ho_Ten;
                    existingKhachHang.Email = khachHang.Email; // Cho phép cập nhật email
                    existingKhachHang.So_Dien_Thoai = khachHang.So_Dien_Thoai;
                    existingKhachHang.GioiTinh = khachHang.GioiTinh;
                    existingKhachHang.Ghi_Chu = khachHang.Ghi_Chu;

                    _context.Khach_Hang.Update(existingKhachHang);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<DiaChi>> GetDiaChiByKhachHangIdAsync(int khachHangId)
        {
            return await _context.Dia_Chi
                .Where(dc => dc.KhachHangDiaChis.Any(khdc => khdc.KhachHang_ID == khachHangId))
                .ToListAsync();
        }

        public async Task<DiaChi> GetDiaChiByIdAsync(int diaChiId)
        {
            return await _context.Dia_Chi
                .FirstOrDefaultAsync(dc => dc.ID_Dia_Chi == diaChiId);
        }

        public async Task<bool> AddDiaChiAsync(DiaChi diaChi, int khachHangId)
        {
            try
            {
                _context.Dia_Chi.Add(diaChi);
                await _context.SaveChangesAsync();

                var khachHangDiaChi = new KhachHangDiaChi
                {
                    ID_Dia_Chi = diaChi.ID_Dia_Chi,
                    KhachHang_ID = khachHangId,
                    Trang_Thai = true
                };
                _context.KhachHang_DiaChi.Add(khachHangDiaChi);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDiaChiAsync(DiaChi diaChi)
        {
            try
            {
                _context.Entry(diaChi).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteDiaChiAsync(int diaChiId, int khachHangId)
        {
            try
            {
                var khachHangDiaChi = await _context.KhachHang_DiaChi
                    .FirstOrDefaultAsync(khdc => khdc.ID_Dia_Chi == diaChiId && khdc.KhachHang_ID == khachHangId);

                if (khachHangDiaChi != null)
                {
                    _context.KhachHang_DiaChi.Remove(khachHangDiaChi);
                }

                var diaChi = await _context.Dia_Chi.FirstOrDefaultAsync(dc => dc.ID_Dia_Chi == diaChiId);
                if (diaChi != null)
                {
                    _context.Dia_Chi.Remove(diaChi);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}