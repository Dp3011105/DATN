using BE.Data;
using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.EntityFrameworkCore;
using System;

namespace BE.Repository
{
    public class DonHangTKRepository : IDonHangTKRepository
    {
        private readonly MyDbContext _context;
        private readonly EmailService _emailService;

        //public DonHangTKRepository(MyDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task<List<HoaDon>> GetHoaDonsByKhachHangAsync(int idKhachHang)
        //{
        //    return await _context.Hoa_Don
        //        .Where(h => h.ID_Khach_Hang == idKhachHang)
        //        .Include(h => h.HinhThucThanhToan)
        //        .Include(h => h.DiaChi)
        //        .Include(h => h.HoaDonVouchers).ThenInclude(hv => hv.Voucher)
        //        .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.SanPham)
        //        .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.Size)
        //        .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.DoNgot)
        //        .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.LuongDa)
        //        .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.HoaDonChiTietToppings).ThenInclude(ht => ht.Topping)
        //        .ToListAsync();
        //}

        //public async Task<HoaDon> GetHoaDonByIdAsync(int idHoaDon)
        //{
        //    return await _context.Hoa_Don
        //        .FirstOrDefaultAsync(h => h.ID_Hoa_Don == idHoaDon);
        //}


        //// Chức nang HUy don hang
        //public async Task UpdateHoaDonAsync(HoaDon hoaDon)
        //{
        //    _context.Hoa_Don.Update(hoaDon);
        //    await _context.SaveChangesAsync();
        //}








        public DonHangTKRepository(MyDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;

        }

        public async Task<List<HoaDon>> GetHoaDonsByKhachHangAsync(int idKhachHang)
        {
            return await _context.Hoa_Don
                .Where(h => h.ID_Khach_Hang == idKhachHang)
                .Include(h => h.HinhThucThanhToan)
                .Include(h => h.DiaChi)
                .Include(h => h.HoaDonVouchers).ThenInclude(hv => hv.Voucher)
                .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.SanPham)
                .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.Size)
                .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.DoNgot)
                .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.LuongDa)
                .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.HoaDonChiTietToppings).ThenInclude(ht => ht.Topping)
                .ToListAsync();
        }

        public async Task<HoaDon> GetHoaDonByIdAsync(int idHoaDon)
        {
            return await _context.Hoa_Don
                .FirstOrDefaultAsync(h => h.ID_Hoa_Don == idHoaDon);
        }

        //public async Task UpdateHoaDonAsync(HoaDon hoaDon)
        //{
        //    _context.Hoa_Don.Update(hoaDon);
        //    await _context.SaveChangesAsync();
        //}

        public async Task UpdateHoaDonAsync(HoaDon hoaDon)
        {
            // Cập nhật đơn hàng
            _context.Hoa_Don.Update(hoaDon);
            await _context.SaveChangesAsync();

            // Chỉ gửi email nếu đơn hàng bị hủy
            if (hoaDon.Trang_Thai == "Huy_Don" && !string.IsNullOrEmpty(hoaDon.LyDoHuyDon))
            {
                // Lấy thông tin khách hàng từ DbContext (đảm bảo lấy được email)
                var khachHang = await _context.Khach_Hang
                    .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang);

                if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
                {
                    try
                    {
                        var subject = "Thông báo hủy đơn hàng thành công";
                        var body = $@"
Kính gửi {khachHang.Ho_Ten ?? "quý khách"},

Đơn hàng của bạn (ID: {hoaDon.ID_Hoa_Don}) đã được hủy thành công.
Lý do hủy: {hoaDon.LyDoHuyDon}

Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.

Trân trọng,
Đội ngũ hỗ trợ";

                        // Gửi mail
                        await _emailService.SendEmailAsync(khachHang.Email, subject, body);
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi, nhưng không throw để không chặn update
                       
                    }
                }
            }
        }


        public async Task<HoaDonVoucher> GetHoaDonVoucherByHoaDonIdAsync(int idHoaDon)
        {
            return await _context.HoaDonVouchers
                .FirstOrDefaultAsync(hdv => hdv.ID_Hoa_Don == idHoaDon);
        }

        public async Task<KhachHangVoucher> GetKhachHangVoucherAsync(int idKhachHang, int idVoucher)
        {
            return await _context.KhachHang_Voucher
                .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == idKhachHang && khv.ID_Voucher == idVoucher);
        }

        public async Task UpdateKhachHangVoucherAsync(KhachHangVoucher khachHangVoucher)
        {
            _context.KhachHang_Voucher.Update(khachHangVoucher);
            await _context.SaveChangesAsync();
        }





    }


}



