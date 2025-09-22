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



        // đoạn này khách hàng hủy thì không hồi số lượng sản phẩm theo nghiệp vụ cũ 
        //        public async Task UpdateHoaDonAsync(HoaDon hoaDon)
        //        {
        //            // Cập nhật đơn hàng
        //            _context.Hoa_Don.Update(hoaDon);
        //            await _context.SaveChangesAsync();

        //            // Chỉ gửi email nếu đơn hàng bị hủy
        //            if (hoaDon.Trang_Thai == "Huy_Don" && !string.IsNullOrEmpty(hoaDon.LyDoHuyDon))
        //            {
        //                // Lấy thông tin khách hàng từ DbContext (đảm bảo lấy được email)
        //                var khachHang = await _context.Khach_Hang
        //                    .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang);

        //                if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
        //                {
        //                    try
        //                    {
        //                        var subject = "Thông báo hủy đơn hàng thành công";
        //                        var body = $@"
        //Kính gửi {khachHang.Ho_Ten ?? "quý khách"},

        //Đơn hàng của bạn (ID: {hoaDon.ID_Hoa_Don}) đã được hủy thành công.
        //Lý do hủy: {hoaDon.LyDoHuyDon}

        //Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.

        //Trân trọng,
        //Đội ngũ hỗ trợ";

        //                        // Gửi mail
        //                        await _emailService.SendEmailAsync(khachHang.Email, subject, body);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        // Log lỗi, nhưng không throw để không chặn update

        //                    }
        //                }
        //            }
        //        }



        // nghiệp vụ mới hồi lại số lượng sản phẩm khi khách hàng hủy đơn


        public async Task UpdateHoaDonAsync(HoaDon hoaDon)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // Cập nhật đơn hàng
                _context.Hoa_Don.Update(hoaDon);

                // Nếu trạng thái là Huy_Don, thực hiện hoàn trả sản phẩm và topping
                if (hoaDon.Trang_Thai == "Huy_Don")
                {
                    // Truy vấn thông tin hóa đơn cùng với chi tiết sản phẩm và topping
                    var invoice = await _context.Hoa_Don
                        .Include(h => h.HoaDonChiTiets)
                            .ThenInclude(hdct => hdct.SanPham)
                        .Include(h => h.HoaDonChiTiets)
                            .ThenInclude(hdct => hdct.HoaDonChiTietToppings)
                            .ThenInclude(hdctt => hdctt.Topping)
                        .Include(h => h.KhachHang)
                            .ThenInclude(kh => kh.KhachHangVouchers)
                        .Include(h => h.HoaDonVouchers)
                            .ThenInclude(hdv => hdv.Voucher)
                        .FirstOrDefaultAsync(h => h.ID_Hoa_Don == hoaDon.ID_Hoa_Don);

                    if (invoice != null)
                    {
                        // Hoàn trả số lượng sản phẩm
                        foreach (var hdct in invoice.HoaDonChiTiets)
                        {
                            if (hdct.SanPham != null)
                            {
                                hdct.SanPham.So_Luong += hdct.So_Luong;
                                _context.San_Pham.Update(hdct.SanPham);
                            }

                            // Hoàn trả số lượng topping
                            foreach (var hdctt in hdct.HoaDonChiTietToppings)
                            {
                                if (hdctt.Topping != null)
                                {
                                    int soLuongTopping = hdctt.So_Luong ?? 0;
                                    hdctt.Topping.So_Luong += soLuongTopping;
                                    _context.Topping.Update(hdctt.Topping);
                                }
                            }
                        }

                        // Hoàn trả voucher nếu có
                        if (invoice.HoaDonVouchers != null && invoice.HoaDonVouchers.Any())
                        {
                            var khachHangVoucher = invoice.KhachHang?.KhachHangVouchers
                                .FirstOrDefault(khv => khv.ID_Voucher == invoice.HoaDonVouchers.First().ID_Voucher);

                            if (khachHangVoucher != null)
                            {
                                khachHangVoucher.Trang_Thai = true;
                                _context.KhachHang_Voucher.Update(khachHangVoucher);
                            }
                        }
                    }
                }

                // Lưu tất cả thay đổi vào cơ sở dữ liệu
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
                            // Ví dụ: _logger.LogError(ex, "Lỗi khi gửi email hủy đơn hàng {ID}", hoaDon.ID_Hoa_Don);
                        }
                    }
                }

                // Commit giao dịch
                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback giao dịch nếu có lỗi
                await tx.RollbackAsync();
                // Log lỗi nếu cần
                // Ví dụ: _logger.LogError(ex, "Lỗi khi cập nhật hóa đơn {ID}", hoaDon.ID_Hoa_Don);
                throw; // Có thể tùy chỉnh xử lý lỗi tùy theo yêu cầu
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



