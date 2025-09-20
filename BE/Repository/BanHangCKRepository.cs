using BE.Data;
using BE.Data;
using BE.DTOs;
using BE.DTOs;
using BE.models;
using BE.models;
using BE.Repository.IRepository;
using BE.Repository.IRepository;
using BE.Service;
using BE.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BE.Repository
{
    public class BanHangCKRepository: IBanHangCKRepository
    {
        private readonly MyDbContext _context; // Replace with your actual DbContext name
        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN"); // Định dạng tiền tệ VND


        public BanHangCKRepository(MyDbContext context, EmailService emailService)
        {
            _context = context;

        }

    

        public async Task<IEnumerable<HinhThucThanhToanDTO>> GetAllHinhThucThanhToan()
        {
            return await _context.Hinh_Thuc_Thanh_Toan
                .Where(httt => httt.Trang_Thai == true) // Chỉ lấy các bản ghi có Trang_Thai = true
                .Select(httt => new HinhThucThanhToanDTO
                {
                    ID_Hinh_Thuc_Thanh_Toan = httt.ID_Hinh_Thuc_Thanh_Toan,
                    Phuong_Thuc_Thanh_Toan = httt.Phuong_Thuc_Thanh_Toan,
                    Cong_Thanh_Toan = httt.Cong_Thanh_Toan,
                    Ghi_Chu = httt.Ghi_Chu,
                    Trang_Thai = httt.Trang_Thai
                })
                .ToListAsync();
        }




        //đoạn code dưới đây thực hiện khi đơn hàng nếu : chưa thanh toán thì lập tức vẫn trừ số lượng sản phẩm vì nếu như 2 người mua thanh toán vnpay cùng 1 lúc thì 
        // sẽ gây ra âm số lượng sản phẩm trong kho
        // thực hiện check id phương thức thanh toán nếu là id 5 vnpay thì trừ kho luôn




        //public async Task<HoaDonBanHangCKDTO> CheckOutTk(HoaDonBanHangCKDTO hoaDonDto)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // 1. Kiểm tra dữ liệu giỏ hàng
        //        if (hoaDonDto.HoaDonChiTiets == null || !hoaDonDto.HoaDonChiTiets.Any())
        //            throw new Exception("Dữ liệu giỏ hàng không hợp lệ hoặc rỗng.");

        //        // 2. Xử lý voucher
        //        decimal giamGiaVoucher = 0;
        //        Voucher voucher = null;
        //        KhachHangVoucher khachHangVoucher = null;

        //        if (hoaDonDto.ID_Voucher.HasValue)
        //        {
        //            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

        //            voucher = await _context.Voucher
        //                .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                       && v.Trang_Thai == true
        //                                       && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));

        //            if (voucher == null)
        //                throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

        //            if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && hoaDonDto.Tong_Tien < voucher.So_Tien_Dat_Yeu_Cau.Value)
        //                throw new Exception($"Tổng tiền hóa đơn không đủ để sử dụng voucher {voucher.Ma_Voucher}.");

        //            if (hoaDonDto.ID_Khach_Hang.HasValue)
        //            {
        //                khachHangVoucher = await _context.KhachHang_Voucher
        //                    .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
        //                                             && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                             && khv.Trang_Thai == true);

        //                if (khachHangVoucher == null)
        //                    throw new Exception($"Khách hàng không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");
        //            }

        //            giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;

        //            if (khachHangVoucher != null)
        //                khachHangVoucher.Trang_Thai = false;
        //        }

        //        // 3. Tạo hóa đơn
        //        var hoaDon = new HoaDon
        //        {
        //            ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
        //            ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
        //            Dia_Chi_Tu_Nhap = hoaDonDto.Dia_Chi_Tu_Nhap,
        //            ID_Phi_Ship = null,
        //            Phi_Ship = hoaDonDto.Phi_Ship, // ✅ phí ship truyền từ client
        //            Ngay_Tao = DateTime.Now,
        //            Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher + hoaDonDto.Phi_Ship,
        //            Ghi_Chu = hoaDonDto.Ghi_Chu,
        //            Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
        //            Loai_Hoa_Don = "Online",
        //            Trang_Thai = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan == 5 ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
        //        };
        //        _context.Hoa_Don.Add(hoaDon);
        //        await _context.SaveChangesAsync();

        //        // 4. Ghi nhận voucher vào hóa đơn (nếu có)
        //        if (voucher != null)
        //        {
        //            var hoaDonVoucher = new HoaDonVoucher
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_Voucher = voucher.ID_Voucher,
        //                Gia_Tri_Giam = giamGiaVoucher
        //            };
        //            _context.HoaDonVouchers.Add(hoaDonVoucher);
        //        }

        //        // 5. Thêm chi tiết hóa đơn
        //        var emailBody = new StringBuilder();
        //        emailBody.AppendLine($"📦 Đơn hàng #{hoaDon.Ma_Hoa_Don}");
        //        emailBody.AppendLine($"Ngày đặt: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
        //        emailBody.AppendLine("-------------------");

        //        foreach (var item in hoaDonDto.HoaDonChiTiets)
        //        {
        //            var sanPham = await _context.San_Pham
        //                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);

        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc đã ngừng bán.");

        //            if (hoaDon.Trang_Thai == "Chua_Thanh_Toan")
        //            {
        //                if (sanPham.So_Luong < item.So_Luong)
        //                    throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ tồn kho.");

        //                sanPham.So_Luong -= item.So_Luong;
        //                _context.San_Pham.Update(sanPham);
        //            }

        //            decimal tongTienChiTiet = item.Gia_Hien_Thi * item.So_Luong;
        //            foreach (var topping in item.HoaDonChiTietToppings)
        //                tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;

        //            var hoaDonChiTiet = new HoaDonChiTiet
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_San_Pham = item.ID_San_Pham,
        //                Gia_San_Pham = item.Gia_Hien_Thi,
        //                Tong_Tien = tongTienChiTiet,
        //                So_Luong = item.So_Luong,
        //                Ghi_Chu = item.Ghi_Chu,
        //                Ngay_Tao = DateTime.Now
        //            };
        //            _context.HoaDon_ChiTiet.Add(hoaDonChiTiet);

        //            // Ghi nội dung email
        //            emailBody.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
        //            emailBody.AppendLine($"Số lượng: {item.So_Luong}");
        //            emailBody.AppendLine($"Giá: {item.Gia_Hien_Thi:N0} đ");
        //            emailBody.AppendLine($"Tổng: {tongTienChiTiet:N0} đ");
        //            emailBody.AppendLine("-------------------");
        //        }

        //        emailBody.AppendLine($"🚚 Phí ship: {hoaDon.Phi_Ship:N0} đ");
        //        emailBody.AppendLine($"💸 Giảm voucher: {giamGiaVoucher:N0} đ");
        //        emailBody.AppendLine($"💰 Thành tiền: {hoaDon.Tong_Tien:N0} đ");

        //        // 6. Trích xuất Gmail từ Ghi_Chu
        //        string extractedEmail = null;
        //        if (!string.IsNullOrEmpty(hoaDonDto.Ghi_Chu))
        //        {
        //            var match = Regex.Match(hoaDonDto.Ghi_Chu,
        //                @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
        //            if (match.Success)
        //                extractedEmail = match.Value;
        //        }

        //        // 7. Gửi email xác nhận
        //        if (!string.IsNullOrEmpty(extractedEmail))
        //        {
        //            using var smtp = new SmtpClient("smtp.gmail.com", 587)
        //            {
        //                Credentials = new NetworkCredential("youremail@gmail.com", "your_app_password"),
        //                EnableSsl = true
        //            };

        //            var mail = new MailMessage
        //            {
        //                From = new MailAddress("youremail@gmail.com", "Cửa hàng Online"),
        //                Subject = $"Xác nhận đơn hàng #{hoaDon.Ma_Hoa_Don}",
        //                Body = emailBody.ToString()
        //            };
        //            mail.To.Add(extractedEmail);

        //            await smtp.SendMailAsync(mail);
        //        }

        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //        return hoaDonDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Lỗi khi xử lý thanh toán: {ex.Message}");
        //    }
        //}



        //public async Task<HoaDonBanHangCKDTO> CheckOutTk(HoaDonBanHangCKDTO hoaDonDto, EmailService emailService)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // 1. Kiểm tra dữ liệu giỏ hàng
        //        if (hoaDonDto.HoaDonChiTiets == null || !hoaDonDto.HoaDonChiTiets.Any())
        //            throw new Exception("Dữ liệu giỏ hàng không hợp lệ hoặc rỗng.");

        //        // 2. Xử lý voucher
        //        decimal giamGiaVoucher = 0;
        //        Voucher voucher = null;
        //        KhachHangVoucher khachHangVoucher = null;

        //        if (hoaDonDto.ID_Voucher.HasValue)
        //        {
        //            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

        //            voucher = await _context.Voucher
        //                .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                       && v.Trang_Thai == true
        //                                       && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));

        //            if (voucher == null)
        //                throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

        //            if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && hoaDonDto.Tong_Tien < voucher.So_Tien_Dat_Yeu_Cau.Value)
        //                throw new Exception($"Tổng tiền hóa đơn không đủ để sử dụng voucher {voucher.Ma_Voucher}.");

        //            if (hoaDonDto.ID_Khach_Hang.HasValue)
        //            {
        //                khachHangVoucher = await _context.KhachHang_Voucher
        //                    .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
        //                                             && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                             && khv.Trang_Thai == true);

        //                if (khachHangVoucher == null)
        //                    throw new Exception($"Khách hàng không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");
        //            }

        //            giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;

        //            if (khachHangVoucher != null)
        //                khachHangVoucher.Trang_Thai = false;
        //        }

        //        // 3. Tạo hóa đơn
        //        var hoaDon = new HoaDon
        //        {
        //            ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
        //            ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
        //            Dia_Chi_Tu_Nhap = hoaDonDto.Dia_Chi_Tu_Nhap,
        //            ID_Phi_Ship = null,
        //            Phi_Ship = hoaDonDto.Phi_Ship, // ✅ phí ship truyền từ client
        //            Ngay_Tao = DateTime.Now,
        //            Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher + hoaDonDto.Phi_Ship,
        //            Ghi_Chu = hoaDonDto.Ghi_Chu,
        //            Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
        //            Loai_Hoa_Don = "Online",
        //            Trang_Thai = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan == 5 ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
        //        };
        //        _context.Hoa_Don.Add(hoaDon);
        //        await _context.SaveChangesAsync();

        //        // 4. Ghi nhận voucher vào hóa đơn (nếu có)
        //        if (voucher != null)
        //        {
        //            var hoaDonVoucher = new HoaDonVoucher
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_Voucher = voucher.ID_Voucher,
        //                Gia_Tri_Giam = giamGiaVoucher
        //            };
        //            _context.HoaDonVouchers.Add(hoaDonVoucher);
        //        }

        //        // 5. Thêm chi tiết hóa đơn
        //        var emailBody = new StringBuilder();
        //        emailBody.AppendLine($"📦 Đơn hàng #{hoaDon.Ma_Hoa_Don}");
        //        emailBody.AppendLine($"Ngày đặt: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
        //        emailBody.AppendLine("-------------------");

        //        foreach (var item in hoaDonDto.HoaDonChiTiets)
        //        {
        //            var sanPham = await _context.San_Pham
        //                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);

        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc đã ngừng bán.");

        //            if (hoaDon.Trang_Thai == "Chua_Thanh_Toan")
        //            {
        //                if (sanPham.So_Luong < item.So_Luong)
        //                    throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ tồn kho.");

        //                sanPham.So_Luong -= item.So_Luong;
        //                _context.San_Pham.Update(sanPham);
        //            }

        //            decimal tongTienChiTiet = item.Gia_Hien_Thi * item.So_Luong;
        //            foreach (var topping in item.HoaDonChiTietToppings)
        //                tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;

        //            var hoaDonChiTiet = new HoaDonChiTiet
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_San_Pham = item.ID_San_Pham,
        //                Gia_San_Pham = item.Gia_Hien_Thi,
        //                Tong_Tien = tongTienChiTiet,
        //                So_Luong = item.So_Luong,
        //                Ghi_Chu = item.Ghi_Chu,
        //                Ngay_Tao = DateTime.Now
        //            };
        //            _context.HoaDon_ChiTiet.Add(hoaDonChiTiet);

        //            // Ghi nội dung email
        //            emailBody.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
        //            emailBody.AppendLine($"Số lượng: {item.So_Luong}");
        //            emailBody.AppendLine($"Giá: {item.Gia_Hien_Thi:N0} đ");
        //            emailBody.AppendLine($"Tổng: {tongTienChiTiet:N0} đ");
        //            emailBody.AppendLine("-------------------");
        //        }

        //        emailBody.AppendLine($"🚚 Phí ship: {hoaDon.Phi_Ship:N0} đ");
        //        emailBody.AppendLine($"💸 Giảm voucher: {giamGiaVoucher:N0} đ");
        //        emailBody.AppendLine($"💰 Thành tiền: {hoaDon.Tong_Tien:N0} đ");

        //        // 6. Trích xuất Gmail từ Ghi_Chu
        //        string extractedEmail = null;
        //        if (!string.IsNullOrEmpty(hoaDonDto.Ghi_Chu))
        //        {
        //            var match = Regex.Match(hoaDonDto.Ghi_Chu,
        //                @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
        //            if (match.Success)
        //                extractedEmail = match.Value;
        //        }

        //        // 7. Gửi email xác nhận bằng EmailService
        //        if (!string.IsNullOrEmpty(extractedEmail))
        //        {
        //            await emailService.SendEmailAsync(
        //                extractedEmail,
        //                $"Xác nhận đơn hàng #{hoaDon.Ma_Hoa_Don}",
        //                emailBody.ToString()
        //            );
        //        }

        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //        return hoaDonDto;
        //    }
        //    //catch (Exception ex)
        //    //{
        //    //    await transaction.RollbackAsync();
        //    //    throw new Exception($"Lỗi khi xử lý thanh toán: {ex.Message}");
        //    //}
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();

        //        var inner = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        throw new Exception($"Lỗi khi xử lý thanh toán: {inner}");
        //    }
        //}



        //public async Task<HoaDonBanHangCKDTO> CheckOutTk(HoaDonBanHangCKDTO hoaDonDto, EmailService emailService)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // 1. Kiểm tra dữ liệu giỏ hàng
        //        if (hoaDonDto.HoaDonChiTiets == null || !hoaDonDto.HoaDonChiTiets.Any())
        //            throw new Exception("Dữ liệu giỏ hàng không hợp lệ hoặc rỗng.");

        //        // 2. Xử lý voucher
        //        decimal giamGiaVoucher = 0;
        //        Voucher voucher = null;
        //        KhachHangVoucher khachHangVoucher = null;

        //        if (hoaDonDto.ID_Voucher.HasValue)
        //        {
        //            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

        //            voucher = await _context.Voucher
        //                .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                       && v.Trang_Thai == true
        //                                       && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));

        //            if (voucher == null)
        //                throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

        //            if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && hoaDonDto.Tong_Tien < voucher.So_Tien_Dat_Yeu_Cau.Value)
        //                throw new Exception($"Tổng tiền hóa đơn không đủ để sử dụng voucher {voucher.Ma_Voucher}.");

        //            if (hoaDonDto.ID_Khach_Hang.HasValue)
        //            {
        //                khachHangVoucher = await _context.KhachHang_Voucher
        //                    .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
        //                                             && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                             && khv.Trang_Thai == true);

        //                if (khachHangVoucher == null)
        //                    throw new Exception($"Khách hàng không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");
        //            }

        //            giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;

        //            if (khachHangVoucher != null)
        //                khachHangVoucher.Trang_Thai = false;
        //        }

        //        // 3. Tạo hóa đơn
        //        var hoaDon = new HoaDon
        //        {
        //            ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
        //            ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
        //            Dia_Chi_Tu_Nhap = hoaDonDto.Dia_Chi_Tu_Nhap,
        //            ID_Phi_Ship = null,
        //            Phi_Ship = hoaDonDto.Phi_Ship,
        //            Ngay_Tao = DateTime.Now,
        //            Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher + hoaDonDto.Phi_Ship,
        //            Ghi_Chu = hoaDonDto.Ghi_Chu,
        //            Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
        //            Loai_Hoa_Don = "Online",
        //            Trang_Thai = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan == 5 ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
        //        };
        //        _context.Hoa_Don.Add(hoaDon);
        //        await _context.SaveChangesAsync();

        //        // 4. Ghi nhận voucher vào hóa đơn (nếu có)
        //        if (voucher != null)
        //        {
        //            var hoaDonVoucher = new HoaDonVoucher
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_Voucher = voucher.ID_Voucher,
        //                Gia_Tri_Giam = giamGiaVoucher
        //            };
        //            _context.HoaDonVouchers.Add(hoaDonVoucher);
        //        }

        //        // 5. Thêm chi tiết hóa đơn
        //        var emailBody = new StringBuilder();
        //        emailBody.AppendLine($"📦 Đơn hàng #{hoaDon.Ma_Hoa_Don}");
        //        emailBody.AppendLine($"Ngày đặt: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
        //        emailBody.AppendLine("-------------------");

        //        foreach (var item in hoaDonDto.HoaDonChiTiets)
        //        {
        //            var sanPham = await _context.San_Pham
        //                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);

        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc đã ngừng bán.");

        //            if (hoaDon.Trang_Thai == "Chua_Thanh_Toan")
        //            {
        //                if (sanPham.So_Luong < item.So_Luong)
        //                    throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ tồn kho.");

        //                sanPham.So_Luong -= item.So_Luong;
        //                _context.San_Pham.Update(sanPham);
        //            }

        //            decimal tongTienChiTiet = item.Gia_Hien_Thi * item.So_Luong;
        //            foreach (var topping in item.HoaDonChiTietToppings)
        //                tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;

        //            var hoaDonChiTiet = new HoaDonChiTiet
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_San_Pham = item.ID_San_Pham,
        //                Gia_San_Pham = item.Gia_Hien_Thi,
        //                Tong_Tien = tongTienChiTiet,
        //                So_Luong = item.So_Luong,
        //                Ghi_Chu = item.Ghi_Chu,
        //                Ngay_Tao = DateTime.Now,
        //                Ma_HoaDon_ChiTiet = Guid.NewGuid().ToString() // ✅ BẮT BUỘC sinh GUID tại đây
        //            };
        //            _context.HoaDon_ChiTiet.Add(hoaDonChiTiet);

        //            // Ghi nội dung email
        //            emailBody.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
        //            emailBody.AppendLine($"Số lượng: {item.So_Luong}");
        //            emailBody.AppendLine($"Giá: {item.Gia_Hien_Thi:N0} đ");
        //            emailBody.AppendLine($"Tổng: {tongTienChiTiet:N0} đ");
        //            emailBody.AppendLine("-------------------");
        //        }

        //        emailBody.AppendLine($"🚚 Phí ship: {hoaDon.Phi_Ship:N0} đ");
        //        emailBody.AppendLine($"💰 Thành tiền: {hoaDon.Tong_Tien:N0} đ");

        //        // 6. Trích xuất Gmail từ Ghi_Chu
        //        string extractedEmail = null;
        //        if (!string.IsNullOrEmpty(hoaDonDto.Ghi_Chu))
        //        {
        //            var match = Regex.Match(hoaDonDto.Ghi_Chu,
        //                @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
        //            if (match.Success)
        //                extractedEmail = match.Value;
        //        }

        //        // 7. Gửi email xác nhận bằng EmailService
        //        if (!string.IsNullOrEmpty(extractedEmail))
        //        {
        //            await emailService.SendEmailAsync(
        //                extractedEmail,
        //                $"Xác nhận đơn hàng #{hoaDon.Ma_Hoa_Don}",
        //                emailBody.ToString()
        //            );
        //        }

        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //        return hoaDonDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Lỗi khi xử lý thanh toán: {ex.Message}");
        //    }
        //}




        public async Task<HoaDonBanHangCKDTO> CheckOutTk(HoaDonBanHangCKDTO hoaDonDto, EmailService emailService)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Kiểm tra dữ liệu giỏ hàng
                if (hoaDonDto.HoaDonChiTiets == null || !hoaDonDto.HoaDonChiTiets.Any())
                    throw new Exception("Dữ liệu giỏ hàng không hợp lệ hoặc rỗng.");

                // 2. Xử lý voucher
                decimal giamGiaVoucher = 0;
                Voucher voucher = null;
                KhachHangVoucher khachHangVoucher = null;

                if (hoaDonDto.ID_Voucher.HasValue)
                {
                    var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

                    voucher = await _context.Voucher
                        .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
                                               && v.Trang_Thai == true
                                               && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));

                    if (voucher == null)
                        throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

                    if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && hoaDonDto.Tong_Tien < voucher.So_Tien_Dat_Yeu_Cau.Value)
                        throw new Exception($"Tổng tiền hóa đơn không đủ để sử dụng voucher {voucher.Ma_Voucher}.");

                    if (hoaDonDto.ID_Khach_Hang.HasValue)
                    {
                        khachHangVoucher = await _context.KhachHang_Voucher
                            .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
                                                     && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
                                                     && khv.Trang_Thai == true);

                        if (khachHangVoucher == null)
                            throw new Exception($"Khách hàng không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");
                    }

                    giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;

                    if (khachHangVoucher != null)
                        khachHangVoucher.Trang_Thai = false;
                }

                // 3. Tạo hóa đơn (DB lưu Tong_Tien riêng, Phi_Ship riêng)
                var hoaDon = new HoaDon
                {
                    ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
                    ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
                    Dia_Chi_Tu_Nhap = hoaDonDto.Dia_Chi_Tu_Nhap,
                    ID_Phi_Ship = null,
                    Phi_Ship = hoaDonDto.Phi_Ship,
                    Ngay_Tao = DateTime.Now,
                    Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher, // DB lưu riêng tổng tiền sản phẩm
                    Ghi_Chu = hoaDonDto.Ghi_Chu,
                    Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
                    Loai_Hoa_Don = "Online",
                    Trang_Thai = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan == 5 ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
                };
                _context.Hoa_Don.Add(hoaDon);
                await _context.SaveChangesAsync();

                // 4. Ghi nhận voucher vào hóa đơn (nếu có)
                if (voucher != null)
                {
                    var hoaDonVoucher = new HoaDonVoucher
                    {
                        ID_Hoa_Don = hoaDon.ID_Hoa_Don,
                        ID_Voucher = voucher.ID_Voucher,
                        Gia_Tri_Giam = giamGiaVoucher
                    };
                    _context.HoaDonVouchers.Add(hoaDonVoucher);
                }

                // 5. Thêm chi tiết hóa đơn
                var emailBody = new StringBuilder();
                emailBody.AppendLine($"📦 Đơn hàng #{hoaDon.Ma_Hoa_Don}");
                emailBody.AppendLine($"Ngày đặt: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
                emailBody.AppendLine("-------------------");

                foreach (var item in hoaDonDto.HoaDonChiTiets)
                {
                    var sanPham = await _context.San_Pham
                        .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);

                    if (sanPham == null)
                        throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc đã ngừng bán.");

                    if (hoaDon.Trang_Thai == "Chua_Thanh_Toan")
                    {
                        if (sanPham.So_Luong < item.So_Luong)
                            throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ tồn kho.");

                        sanPham.So_Luong -= item.So_Luong;
                        _context.San_Pham.Update(sanPham);
                    }

                    decimal tongTienChiTiet = item.Gia_Hien_Thi * item.So_Luong;
                    foreach (var topping in item.HoaDonChiTietToppings)
                        tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;

                    var hoaDonChiTiet = new HoaDonChiTiet
                    {
                        ID_Hoa_Don = hoaDon.ID_Hoa_Don,
                        ID_San_Pham = item.ID_San_Pham,
                        Gia_San_Pham = item.Gia_Hien_Thi,
                        Tong_Tien = tongTienChiTiet, // DB lưu riêng
                        So_Luong = item.So_Luong,
                        Ghi_Chu = item.Ghi_Chu,
                        Ngay_Tao = DateTime.Now,
                        Ma_HoaDon_ChiTiet = Guid.NewGuid().ToString()
                    };
                    _context.HoaDon_ChiTiet.Add(hoaDonChiTiet);

                    // Ghi email (tính tổng = sản phẩm + phi ship - voucher)
                    emailBody.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
                    emailBody.AppendLine($"Số lượng: {item.So_Luong}");
                    emailBody.AppendLine($"Giá: {item.Gia_Hien_Thi:N0} đ");
                    emailBody.AppendLine($"Tổng: {tongTienChiTiet:N0} đ");
                    emailBody.AppendLine("-------------------");
                }

                // Tổng tiền hiển thị cho khách = DB.Tong_Tien + Phi_Ship - Voucher
                decimal tongTienHienThi = (decimal)hoaDon.Tong_Tien + (decimal)hoaDon.Phi_Ship;


                emailBody.AppendLine($"🚚 Phí ship: {hoaDon.Phi_Ship:N0} đ");
                emailBody.AppendLine($"💸 Giảm voucher: {giamGiaVoucher:N0} đ");
                emailBody.AppendLine($"💰 Thành tiền: {tongTienHienThi - giamGiaVoucher:N0} đ");

                // 6. Trích xuất Gmail từ Ghi_Chu
                string extractedEmail = null;
                if (!string.IsNullOrEmpty(hoaDonDto.Ghi_Chu))
                {
                    var match = Regex.Match(hoaDonDto.Ghi_Chu,
                        @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
                    if (match.Success)
                        extractedEmail = match.Value;
                }

                // 7. Gửi email xác nhận
                if (!string.IsNullOrEmpty(extractedEmail))
                {
                    await emailService.SendEmailAsync(
                        extractedEmail,
                        $"Xác nhận đơn hàng #{hoaDon.Ma_Hoa_Don}",
                        emailBody.ToString()
                    );
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return hoaDonDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi khi xử lý thanh toán: {ex.Message}");
            }
        }




        public async Task<List<VoucherBanHangCKDto>> GetAllVouchersAsync()
        {
            return await _context.Voucher
                .Select(v => new VoucherBanHangCKDto
                {
                    ID_Voucher = v.ID_Voucher,
                    Ma_Voucher = v.Ma_Voucher,
                    Ten = v.Ten,
                    So_Luong = v.So_Luong,
                    Gia_Tri_Giam = v.Gia_Tri_Giam,
                    So_Tien_Dat_Yeu_Cau = v.So_Tien_Dat_Yeu_Cau,
                    Ngay_Bat_Dau = v.Ngay_Bat_Dau,
                    Ngay_Ket_Thuc = v.Ngay_Ket_Thuc,
                    Trang_Thai = v.Trang_Thai
                })
                .ToListAsync();
        }

        public async Task<Voucher> GetVoucherByIdAsync(int id)
        {
            return await _context.Voucher.FindAsync(id);
        }

        public async Task<bool> UpdateVoucherAsync(Voucher voucher)
        {
            _context.Voucher.Update(voucher);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


    }
}
