using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace BE.Repository
{
    public class BanHangTKRepository : IBanHangTKRepository
    {
        private readonly MyDbContext _context; // Replace with your actual DbContext name
        private readonly EmailService _emailService;
        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN"); // Định dạng tiền tệ VND


        public BanHangTKRepository(MyDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;

        }

        public async Task<List<DiaChiBanHangDTO>> GetActiveDiaChiByKhachHangId(int idKhachHang)
        {
            return await _context.KhachHang_DiaChi
                .Where(kdc => kdc.KhachHang_ID == idKhachHang && kdc.Trang_Thai == true)
                .Include(kdc => kdc.DiaChi)
                .Where(kdc => kdc.DiaChi.Trang_Thai == true)
                .Select(kdc => new DiaChiBanHangDTO
                {
                    ID_Dia_Chi = kdc.ID_Dia_Chi,
                    Dia_Chi = kdc.DiaChi.Dia_Chi,
                    Tinh_Thanh = kdc.DiaChi.Tinh_Thanh,
                    Ghi_Chu = kdc.DiaChi.Ghi_Chu,
                    Ghi_Chu_KhachHang = kdc.Ghi_Chu
                })
                .ToListAsync();
        }

        public async Task<int> AddDiaChiForKhachHang(int idKhachHang, CreateDiaChiBanHangDTO dto)
        {
            var newDiaChi = new DiaChi
            {
                Dia_Chi = dto.Dia_Chi,
                Tinh_Thanh = dto.Tinh_Thanh,
                Ghi_Chu = dto.Ghi_Chu,
                Trang_Thai = true
            };

            _context.Dia_Chi.Add(newDiaChi);
            await _context.SaveChangesAsync();

            var newKhachHangDiaChi = new KhachHangDiaChi
            {
                ID_Dia_Chi = newDiaChi.ID_Dia_Chi,
                KhachHang_ID = idKhachHang,
                Ghi_Chu = dto.Ghi_Chu_KhachHang,
                Trang_Thai = true
            };

            _context.KhachHang_DiaChi.Add(newKhachHangDiaChi);
            await _context.SaveChangesAsync();

            return newDiaChi.ID_Dia_Chi;
        }

        public async Task<bool> UpdateDiaChi(int idKhachHang, int idDiaChi, UpdateDiaChiBanHangDTO dto)
        {
            var khdc = await _context.KhachHang_DiaChi
                .FirstOrDefaultAsync(k => k.KhachHang_ID == idKhachHang && k.ID_Dia_Chi == idDiaChi);

            if (khdc == null) return false;

            var diaChi = await _context.Dia_Chi.FindAsync(idDiaChi);
            if (diaChi == null) return false;

            if (!string.IsNullOrEmpty(dto.Dia_Chi)) diaChi.Dia_Chi = dto.Dia_Chi;
            if (!string.IsNullOrEmpty(dto.Tinh_Thanh)) diaChi.Tinh_Thanh = dto.Tinh_Thanh;
            if (!string.IsNullOrEmpty(dto.Ghi_Chu)) diaChi.Ghi_Chu = dto.Ghi_Chu;
            if (dto.Trang_Thai.HasValue) diaChi.Trang_Thai = dto.Trang_Thai;
            if (dto.Trang_Thai.HasValue) khdc.Trang_Thai = dto.Trang_Thai;
            if (!string.IsNullOrEmpty(dto.Ghi_Chu_KhachHang)) khdc.Ghi_Chu = dto.Ghi_Chu_KhachHang;

            await _context.SaveChangesAsync();
            return true;
        }



        //public async Task<IEnumerable<VoucherBanHangDTO>> GetVouchersByKhachHang(int idKhachHang)
        //{
        //    return await _context.KhachHang_Voucher
        //        .Where(khv => khv.ID_Khach_Hang == idKhachHang && khv.Trang_Thai == true && khv.Voucher.Trang_Thai == true)
        //        .Select(khv => new VoucherBanHangDTO
        //        {
        //            ID_Voucher = khv.Voucher.ID_Voucher,
        //            Ma_Voucher = khv.Voucher.Ma_Voucher,
        //            Ten = khv.Voucher.Ten,
        //            Gia_Tri_Giam = khv.Voucher.Gia_Tri_Giam,
        //            So_Tien_Dat_Yeu_Cau = khv.Voucher.So_Tien_Dat_Yeu_Cau,
        //            Ngay_Bat_Dau = khv.Voucher.Ngay_Bat_Dau,
        //            Ngay_Ket_Thuc = khv.Voucher.Ngay_Ket_Thuc,
        //            Trang_Thai = khv.Voucher.Trang_Thai
        //        })
        //        .ToListAsync();
        //}
        public async Task<IEnumerable<VoucherBanHangDTO>> GetVouchersByKhachHang(int idKhachHang)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            return await _context.KhachHang_Voucher
                .Include(khv => khv.Voucher)
                .Where(khv => khv.ID_Khach_Hang == idKhachHang
                              && khv.Trang_Thai == true
                              && khv.Voucher.Trang_Thai == true
                              && (khv.Voucher.Ngay_Ket_Thuc == null || khv.Voucher.Ngay_Ket_Thuc >= currentTime))
                .Select(khv => new VoucherBanHangDTO
                {
                    ID_Voucher = khv.Voucher.ID_Voucher,
                    Ma_Voucher = khv.Voucher.Ma_Voucher,
                    Ten = khv.Voucher.Ten,
                    So_Luong = khv.Voucher.So_Luong,
                    Gia_Tri_Giam = khv.Voucher.Gia_Tri_Giam,
                    So_Tien_Dat_Yeu_Cau = khv.Voucher.So_Tien_Dat_Yeu_Cau,
                    Ngay_Bat_Dau = khv.Voucher.Ngay_Bat_Dau,
                    Ngay_Ket_Thuc = khv.Voucher.Ngay_Ket_Thuc,
                    Trang_Thai = khv.Voucher.Trang_Thai
                })
                .ToListAsync();
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



        ///Logic check out dưới là trừ số lượng , trừ Topping , trừ voucher


        //public async Task<HoaDonBanHangTKDTO> CheckOutTk(HoaDonBanHangTKDTO hoaDonDto)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        // Kiểm tra dữ liệu giỏ hàng
        //        if (hoaDonDto.HoaDonChiTiets == null || !hoaDonDto.HoaDonChiTiets.Any())
        //            throw new Exception("Dữ liệu giỏ hàng không hợp lệ hoặc rỗng.");

        //        // Kiểm tra và áp dụng voucher nếu có
        //        decimal giamGiaVoucher = 0;
        //        Voucher voucher = null;
        //        KhachHangVoucher khachHangVoucher = null;
        //        if (hoaDonDto.ID_Voucher.HasValue)
        //        {
        //            if (!hoaDonDto.ID_Khach_Hang.HasValue)
        //                throw new Exception("Phải cung cấp ID_Khach_Hang khi sử dụng voucher.");

        //            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

        //            // Kiểm tra voucher
        //            voucher = await _context.Voucher
        //                .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                       && v.Trang_Thai == true
        //                                       && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));
        //            if (voucher == null)
        //                throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

        //            // Kiểm tra số lượng voucher
        //            if (!voucher.So_Luong.HasValue || voucher.So_Luong < 1)
        //                throw new Exception($"Voucher {voucher.Ma_Voucher} đã hết số lượng.");

        //            // Kiểm tra điều kiện số tiền đạt yêu cầu của voucher
        //            if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && hoaDonDto.Tong_Tien < voucher.So_Tien_Dat_Yeu_Cau.Value)
        //                throw new Exception($"Tổng tiền hóa đơn không đủ để sử dụng voucher {voucher.Ma_Voucher}.");

        //            // Kiểm tra KhachHangVoucher
        //            khachHangVoucher = await _context.KhachHang_Voucher
        //                .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
        //                                         && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                         && khv.Trang_Thai == true);
        //            if (khachHangVoucher == null)
        //                throw new Exception($"Khách hàng {hoaDonDto.ID_Khach_Hang} không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");

        //            giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;
        //            //voucher.So_Luong -= 1; // Trừ số lượng voucher
        //            khachHangVoucher.Trang_Thai = false; // Cập nhật Trang_Thai thành false
        //        }

        //        // Tạo hóa đơn
        //        var hoaDon = new HoaDon
        //        {
        //            ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
        //            ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
        //            ID_Dia_Chi = hoaDonDto.ID_Dia_Chi,
        //            ID_Phi_Ship = null,
        //            Phi_Ship = 0,
        //            Ngay_Tao = DateTime.Now,
        //            Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher,
        //            Ghi_Chu = hoaDonDto.Ghi_Chu,
        //            Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
        //            Loai_Hoa_Don = "Online",
        //            Trang_Thai = "Chua_Xac_Nhan"
        //        };

        //        _context.Hoa_Don.Add(hoaDon);
        //        await _context.SaveChangesAsync();

        //        // Lưu voucher vào HoaDonVoucher nếu có
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

        //        // Xử lý chi tiết hóa đơn và trừ số lượng
        //        var emailChiTietSanPham = new StringBuilder();
        //        foreach (var item in hoaDonDto.HoaDonChiTiets)
        //        {
        //            // Kiểm tra sản phẩm
        //            var sanPham = await _context.San_Pham
        //                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);
        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc không hoạt động.");
        //            if (sanPham.So_Luong < item.So_Luong)
        //                throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ số lượng.");

        //            sanPham.So_Luong -= item.So_Luong;

        //            // Ánh xạ tên Size, Lượng Đá, Độ Ngọt sang ID
        //            int? idSize = null;
        //            if (!string.IsNullOrEmpty(item.Ten_Size))
        //            {
        //                var size = await _context.Size.FirstOrDefaultAsync(s => s.SizeName == item.Ten_Size);
        //                if (size == null)
        //                    throw new Exception($"Size {item.Ten_Size} không tồn tại.");
        //                idSize = size.ID_Size;
        //            }

        //            int? idLuongDa = null;
        //            if (!string.IsNullOrEmpty(item.Ten_LuongDa))
        //            {
        //                var luongDa = await _context.LuongDa.FirstOrDefaultAsync(ld => ld.Ten_LuongDa == item.Ten_LuongDa);
        //                if (luongDa == null)
        //                    throw new Exception($"Lượng đá {item.Ten_LuongDa} không tồn tại.");
        //                idLuongDa = luongDa.ID_LuongDa;
        //            }

        //            int? idDoNgot = null;
        //            if (!string.IsNullOrEmpty(item.Ten_DoNgot))
        //            {
        //                var doNgot = await _context.DoNgot.FirstOrDefaultAsync(dn => dn.Muc_Do == item.Ten_DoNgot);
        //                if (doNgot == null)
        //                    throw new Exception($"Độ ngọt {item.Ten_DoNgot} không tồn tại.");
        //                idDoNgot = doNgot.ID_DoNgot;
        //            }

        //            // Tính giá thêm size (giả định bảng Size có cột Gia_Them)
        //            decimal giaThemSize = 0;
        //            if (idSize.HasValue)
        //            {
        //                var size = await _context.Size.FirstOrDefaultAsync(s => s.ID_Size == idSize.Value);
        //                giaThemSize = size?.Gia ?? 0;
        //            }

        //            // Tính tổng tiền cho chi tiết hóa đơn
        //            decimal tongTienChiTiet = (item.Gia_Hien_Thi + giaThemSize) * item.So_Luong;
        //            foreach (var topping in item.HoaDonChiTietToppings)
        //            {
        //                tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;
        //            }

        //            // Cập nhật ID và tổng tiền trong DTO
        //            item.ID_Size = idSize;
        //            item.ID_SanPham_DoNgot = idDoNgot;
        //            item.ID_LuongDa = idLuongDa;
        //            item.Gia_Them_Size = giaThemSize;
        //            item.Gia_San_Pham = item.Gia_Hien_Thi;
        //            item.Tong_Tien = tongTienChiTiet;
        //            item.Ma_HoaDon_ChiTiet = Guid.NewGuid().ToString(); // Tạo mã ngẫu nhiên

        //            // Tạo chi tiết hóa đơn
        //            var hoaDonChiTiet = new HoaDonChiTiet
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_San_Pham = item.ID_San_Pham,
        //                ID_Size = idSize,
        //                ID_SanPham_DoNgot = idDoNgot,
        //                ID_LuongDa = idLuongDa,
        //                Ma_HoaDon_ChiTiet = item.Ma_HoaDon_ChiTiet,
        //                Gia_Them_Size = giaThemSize,
        //                Gia_San_Pham = item.Gia_Hien_Thi,
        //                Tong_Tien = tongTienChiTiet,
        //                So_Luong = item.So_Luong,
        //                Ghi_Chu = item.Ghi_Chu,
        //                Ngay_Tao = DateTime.Now
        //            };

        //            _context.HoaDon_ChiTiet.Add(hoaDonChiTiet);
        //            await _context.SaveChangesAsync(); // Lưu để có ID_HoaDon_ChiTiet

        //            // Xử lý topping
        //            if (item.HoaDonChiTietToppings != null && item.HoaDonChiTietToppings.Any())
        //            {
        //                emailChiTietSanPham.AppendLine("Topping:");
        //                foreach (var toppingDto in item.HoaDonChiTietToppings)
        //                {
        //                    var topping = await _context.Topping
        //                        .FirstOrDefaultAsync(t => t.ID_Topping == toppingDto.ID_Topping && t.Trang_Thai == true);
        //                    if (topping == null)
        //                        throw new Exception($"Topping {toppingDto.Ten_Topping} không tồn tại hoặc không hoạt động.");
        //                    if (topping.So_Luong < toppingDto.So_Luong)
        //                        throw new Exception($"Topping {topping.Ten} không đủ số lượng.");

        //                    topping.So_Luong -= toppingDto.So_Luong;

        //                    var hoaDonChiTietTopping = new HoaDonChiTietTopping
        //                    {
        //                        ID_HoaDon_ChiTiet = hoaDonChiTiet.ID_HoaDon_ChiTiet,
        //                        ID_Topping = toppingDto.ID_Topping,
        //                        So_Luong = toppingDto.So_Luong,
        //                        Gia_Topping = toppingDto.Gia_Topping
        //                    };

        //                    _context.HoaDonChiTiet_Topping.Add(hoaDonChiTietTopping);

        //                    // Thêm thông tin topping vào email
        //                    emailChiTietSanPham.AppendLine($" - {topping.Ten}: {toppingDto.So_Luong} x {toppingDto.Gia_Topping.ToString("C", _vnCulture)}");
        //                }
        //            }

        //            // Thêm thông tin sản phẩm vào email
        //            emailChiTietSanPham.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
        //            emailChiTietSanPham.AppendLine($"Số lượng: {item.So_Luong}");
        //            emailChiTietSanPham.AppendLine($"Giá sản phẩm: {item.Gia_Hien_Thi.ToString("C", _vnCulture)}");
        //            if (giaThemSize > 0)
        //                emailChiTietSanPham.AppendLine($"Giá thêm size: {giaThemSize.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine($"Tổng tiền: {tongTienChiTiet.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine("-------------------");
        //        }

        //        await _context.SaveChangesAsync();

        //        // Gửi email thông báo hóa đơn
        //        if (hoaDon.ID_Khach_Hang.HasValue)
        //        {
        //            var khachHang = await _context.Khach_Hang
        //                .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang.Value);
        //            if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
        //            {
        //                string linkTraCuu = $"https://yourdomain.com/hoa-don/tra-cuu/{hoaDon.Ma_Hoa_Don}";

        //                var emailBody = new StringBuilder();
        //                emailBody.AppendLine("Hóa đơn của bạn đã được tạo thành công!");
        //                emailBody.AppendLine($"Mã hóa đơn: {hoaDon.Ma_Hoa_Don}");
        //                emailBody.AppendLine($"Ngày tạo: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
        //                emailBody.AppendLine($"Trạng thái: Chưa Xác Nhận");
        //                emailBody.AppendLine($"Tra cứu chi tiết tại: {linkTraCuu}");
        //                emailBody.AppendLine("Cảm ơn bạn đã mua hàng!");

        //                await _emailService.SendEmailAsync(khachHang.Email, "Thông tin hóa đơn", emailBody.ToString());
        //            }
        //        }

        //        await transaction.CommitAsync();

        //        return hoaDonDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Lỗi khi xử lý thanh toán: {ex.Message}");
        //    }
        //}



        //Code dưới không hỗ trợ VNPAY 

        //public async Task<HoaDonBanHangTKDTO> CheckOutTk(HoaDonBanHangTKDTO hoaDonDto)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        // Kiểm tra dữ liệu giỏ hàng
        //        if (hoaDonDto.HoaDonChiTiets == null || !hoaDonDto.HoaDonChiTiets.Any())
        //            throw new Exception("Dữ liệu giỏ hàng không hợp lệ hoặc rỗng.");

        //        // Kiểm tra và áp dụng voucher nếu có
        //        decimal giamGiaVoucher = 0;
        //        Voucher voucher = null;
        //        KhachHangVoucher khachHangVoucher = null;
        //        if (hoaDonDto.ID_Voucher.HasValue)
        //        {
        //            if (!hoaDonDto.ID_Khach_Hang.HasValue)
        //                throw new Exception("Phải cung cấp ID_Khach_Hang khi sử dụng voucher.");

        //            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

        //            // Kiểm tra voucher
        //            voucher = await _context.Voucher
        //                .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                       && v.Trang_Thai == true
        //                                       && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));
        //            if (voucher == null)
        //                throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

        //            // Kiểm tra điều kiện số tiền đạt yêu cầu của voucher
        //            if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && hoaDonDto.Tong_Tien < voucher.So_Tien_Dat_Yeu_Cau.Value)
        //                throw new Exception($"Tổng tiền hóa đơn không đủ để sử dụng voucher {voucher.Ma_Voucher}.");

        //            // Kiểm tra KhachHangVoucher
        //            khachHangVoucher = await _context.KhachHang_Voucher
        //                .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
        //                                         && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                         && khv.Trang_Thai == true);
        //            if (khachHangVoucher == null)
        //                throw new Exception($"Khách hàng {hoaDonDto.ID_Khach_Hang} không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");

        //            giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;
        //            khachHangVoucher.Trang_Thai = false; // Cập nhật Trang_Thai thành false
        //        }

        //        // Tạo hóa đơn
        //        var hoaDon = new HoaDon
        //        {
        //            ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
        //            ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
        //            ID_Dia_Chi = hoaDonDto.ID_Dia_Chi,
        //            ID_Phi_Ship = null,
        //            Phi_Ship = 0,
        //            Ngay_Tao = DateTime.Now,
        //            Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher,
        //            Ghi_Chu = hoaDonDto.Ghi_Chu,
        //            Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
        //            Loai_Hoa_Don = "Online",
        //            Trang_Thai = "Chua_Xac_Nhan"
        //        };

        //        _context.Hoa_Don.Add(hoaDon);
        //        await _context.SaveChangesAsync();

        //        // Lưu voucher vào HoaDonVoucher nếu có
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

        //        // Xử lý chi tiết hóa đơn
        //        var emailChiTietSanPham = new StringBuilder();
        //        foreach (var item in hoaDonDto.HoaDonChiTiets)
        //        {
        //            // Kiểm tra sản phẩm
        //            var sanPham = await _context.San_Pham
        //                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);
        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc không hoạt động.");

        //            // Ánh xạ tên Size, Lượng Đá, Độ Ngọt sang ID
        //            int? idSize = null;
        //            if (!string.IsNullOrEmpty(item.Ten_Size))
        //            {
        //                var size = await _context.Size.FirstOrDefaultAsync(s => s.SizeName == item.Ten_Size);
        //                if (size == null)
        //                    throw new Exception($"Size {item.Ten_Size} không tồn tại.");
        //                idSize = size.ID_Size;
        //            }

        //            int? idLuongDa = null;
        //            if (!string.IsNullOrEmpty(item.Ten_LuongDa))
        //            {
        //                var luongDa = await _context.LuongDa.FirstOrDefaultAsync(ld => ld.Ten_LuongDa == item.Ten_LuongDa);
        //                if (luongDa == null)
        //                    throw new Exception($"Lượng đá {item.Ten_LuongDa} không tồn tại.");
        //                idLuongDa = luongDa.ID_LuongDa;
        //            }

        //            int? idDoNgot = null;
        //            if (!string.IsNullOrEmpty(item.Ten_DoNgot))
        //            {
        //                var doNgot = await _context.DoNgot.FirstOrDefaultAsync(dn => dn.Muc_Do == item.Ten_DoNgot);
        //                if (doNgot == null)
        //                    throw new Exception($"Độ ngọt {item.Ten_DoNgot} không tồn tại.");
        //                idDoNgot = doNgot.ID_DoNgot;
        //            }

        //            // Tính giá thêm size (giả định bảng Size có cột Gia_Them)
        //            decimal giaThemSize = 0;
        //            if (idSize.HasValue)
        //            {
        //                var size = await _context.Size.FirstOrDefaultAsync(s => s.ID_Size == idSize.Value);
        //                giaThemSize = size?.Gia ?? 0;
        //            }

        //            // Tính tổng tiền cho chi tiết hóa đơn
        //            decimal tongTienChiTiet = (item.Gia_Hien_Thi + giaThemSize) * item.So_Luong;
        //            foreach (var topping in item.HoaDonChiTietToppings)
        //            {
        //                tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;
        //            }

        //            // Cập nhật ID và tổng tiền trong DTO
        //            item.ID_Size = idSize;
        //            item.ID_SanPham_DoNgot = idDoNgot;
        //            item.ID_LuongDa = idLuongDa;
        //            item.Gia_Them_Size = giaThemSize;
        //            item.Gia_San_Pham = item.Gia_Hien_Thi;
        //            item.Tong_Tien = tongTienChiTiet;
        //            item.Ma_HoaDon_ChiTiet = Guid.NewGuid().ToString(); // Tạo mã ngẫu nhiên

        //            // Tạo chi tiết hóa đơn
        //            var hoaDonChiTiet = new HoaDonChiTiet
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_San_Pham = item.ID_San_Pham,
        //                ID_Size = idSize,
        //                ID_SanPham_DoNgot = idDoNgot,
        //                ID_LuongDa = idLuongDa,
        //                Ma_HoaDon_ChiTiet = item.Ma_HoaDon_ChiTiet,
        //                Gia_Them_Size = giaThemSize,
        //                Gia_San_Pham = item.Gia_Hien_Thi,
        //                Tong_Tien = tongTienChiTiet,
        //                So_Luong = item.So_Luong,
        //                Ghi_Chu = item.Ghi_Chu,
        //                Ngay_Tao = DateTime.Now
        //            };

        //            _context.HoaDon_ChiTiet.Add(hoaDonChiTiet);
        //            await _context.SaveChangesAsync(); // Lưu để có ID_HoaDon_ChiTiet

        //            // Xử lý topping
        //            if (item.HoaDonChiTietToppings != null && item.HoaDonChiTietToppings.Any())
        //            {
        //                emailChiTietSanPham.AppendLine("Topping:");
        //                foreach (var toppingDto in item.HoaDonChiTietToppings)
        //                {
        //                    var topping = await _context.Topping
        //                        .FirstOrDefaultAsync(t => t.ID_Topping == toppingDto.ID_Topping && t.Trang_Thai == true);
        //                    if (topping == null)
        //                        throw new Exception($"Topping {toppingDto.Ten_Topping} không tồn tại hoặc không hoạt động.");

        //                    var hoaDonChiTietTopping = new HoaDonChiTietTopping
        //                    {
        //                        ID_HoaDon_ChiTiet = hoaDonChiTiet.ID_HoaDon_ChiTiet,
        //                        ID_Topping = toppingDto.ID_Topping,
        //                        So_Luong = toppingDto.So_Luong,
        //                        Gia_Topping = toppingDto.Gia_Topping
        //                    };

        //                    _context.HoaDonChiTiet_Topping.Add(hoaDonChiTietTopping);

        //                    // Thêm thông tin topping vào email
        //                    emailChiTietSanPham.AppendLine($" - {topping.Ten}: {toppingDto.So_Luong} x {toppingDto.Gia_Topping.ToString("C", _vnCulture)}");
        //                }
        //            }

        //            // Thêm thông tin sản phẩm vào email
        //            emailChiTietSanPham.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
        //            emailChiTietSanPham.AppendLine($"Số lượng: {item.So_Luong}");
        //            emailChiTietSanPham.AppendLine($"Giá sản phẩm: {item.Gia_Hien_Thi.ToString("C", _vnCulture)}");
        //            if (giaThemSize > 0)
        //                emailChiTietSanPham.AppendLine($"Giá thêm size: {giaThemSize.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine($"Tổng tiền: {tongTienChiTiet.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine("-------------------");
        //        }

        //        await _context.SaveChangesAsync();

        //        // Gửi email thông báo hóa đơn
        //        if (hoaDon.ID_Khach_Hang.HasValue)
        //        {
        //            var khachHang = await _context.Khach_Hang
        //                .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang.Value);
        //            if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
        //            {
        //                string linkTraCuu = $"https://yourdomain.com/hoa-don/tra-cuu/{hoaDon.Ma_Hoa_Don}";

        //                var emailBody = new StringBuilder();
        //                emailBody.AppendLine("Hóa đơn của bạn đã được tạo thành công!");
        //                emailBody.AppendLine($"Mã hóa đơn: {hoaDon.Ma_Hoa_Don}");
        //                emailBody.AppendLine($"Ngày tạo: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
        //                emailBody.AppendLine($"Trạng thái: Chưa Xác Nhận");
        //                emailBody.AppendLine($"Tra cứu chi tiết tại: {linkTraCuu}");
        //                emailBody.AppendLine("Cảm ơn bạn đã mua hàng!");

        //                await _emailService.SendEmailAsync(khachHang.Email, "Thông tin hóa đơn", emailBody.ToString());
        //            }
        //        }

        //        await transaction.CommitAsync();

        //        return hoaDonDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Lỗi khi xử lý thanh toán: {ex.Message}");
        //    }
        //}







        //public async Task<HoaDonBanHangTKDTO> CheckOutTk(HoaDonBanHangTKDTO hoaDonDto)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        // Kiểm tra dữ liệu giỏ hàng
        //        if (hoaDonDto.HoaDonChiTiets == null || !hoaDonDto.HoaDonChiTiets.Any())
        //            throw new Exception("Dữ liệu giỏ hàng không hợp lệ hoặc rỗng.");

        //        // Kiểm tra và áp dụng voucher nếu có
        //        decimal giamGiaVoucher = 0;
        //        Voucher voucher = null;
        //        KhachHangVoucher khachHangVoucher = null;
        //        if (hoaDonDto.ID_Voucher.HasValue)
        //        {
        //            if (!hoaDonDto.ID_Khach_Hang.HasValue)
        //                throw new Exception("Phải cung cấp ID_Khach_Hang khi sử dụng voucher.");

        //            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        //            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

        //            // Kiểm tra voucher
        //            voucher = await _context.Voucher
        //                .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                       && v.Trang_Thai == true
        //                                       && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));
        //            if (voucher == null)
        //                throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

        //            // Kiểm tra điều kiện số tiền đạt yêu cầu của voucher
        //            if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && hoaDonDto.Tong_Tien < voucher.So_Tien_Dat_Yeu_Cau.Value)
        //                throw new Exception($"Tổng tiền hóa đơn không đủ để sử dụng voucher {voucher.Ma_Voucher}.");

        //            // Kiểm tra KhachHangVoucher
        //            khachHangVoucher = await _context.KhachHang_Voucher
        //                .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
        //                                         && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                         && khv.Trang_Thai == true);
        //            if (khachHangVoucher == null)
        //                throw new Exception($"Khách hàng {hoaDonDto.ID_Khach_Hang} không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");

        //            giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;
        //            khachHangVoucher.Trang_Thai = false; // Cập nhật Trang_Thai thành false
        //        }

        //        // Tạo hóa đơn
        //        var hoaDon = new HoaDon
        //        {
        //            ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
        //            ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
        //            ID_Dia_Chi = hoaDonDto.ID_Dia_Chi,
        //            ID_Phi_Ship = null,
        //            Phi_Ship = 0,
        //            Ngay_Tao = DateTime.Now,
        //            Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher,
        //            Ghi_Chu = hoaDonDto.Ghi_Chu,
        //            Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
        //            Loai_Hoa_Don = "Online",
        //            Trang_Thai = hoaDonDto.Ghi_Chu == "VNPAY" ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
        //        };

        //        _context.Hoa_Don.Add(hoaDon);
        //        await _context.SaveChangesAsync();

        //        // Lưu voucher vào HoaDonVoucher nếu có
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

        //        // Xử lý chi tiết hóa đơn
        //        var emailChiTietSanPham = new StringBuilder();
        //        foreach (var item in hoaDonDto.HoaDonChiTiets)
        //        {
        //            // Kiểm tra sản phẩm
        //            var sanPham = await _context.San_Pham
        //                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);
        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc không hoạt động.");

        //            // Ánh xạ tên Size, Lượng Đá, Độ Ngọt sang ID
        //            int? idSize = null;
        //            if (!string.IsNullOrEmpty(item.Ten_Size))
        //            {
        //                var size = await _context.Size.FirstOrDefaultAsync(s => s.SizeName == item.Ten_Size);
        //                if (size == null)
        //                    throw new Exception($"Size {item.Ten_Size} không tồn tại.");
        //                idSize = size.ID_Size;
        //            }

        //            int? idLuongDa = null;
        //            if (!string.IsNullOrEmpty(item.Ten_LuongDa))
        //            {
        //                var luongDa = await _context.LuongDa.FirstOrDefaultAsync(ld => ld.Ten_LuongDa == item.Ten_LuongDa);
        //                if (luongDa == null)
        //                    throw new Exception($"Lượng đá {item.Ten_LuongDa} không tồn tại.");
        //                idLuongDa = luongDa.ID_LuongDa;
        //            }

        //            int? idDoNgot = null;
        //            if (!string.IsNullOrEmpty(item.Ten_DoNgot))
        //            {
        //                var doNgot = await _context.DoNgot.FirstOrDefaultAsync(dn => dn.Muc_Do == item.Ten_DoNgot);
        //                if (doNgot == null)
        //                    throw new Exception($"Độ ngọt {item.Ten_DoNgot} không tồn tại.");
        //                idDoNgot = doNgot.ID_DoNgot;
        //            }

        //            // Tính giá thêm size (giả định bảng Size có cột Gia_Them)
        //            decimal giaThemSize = 0;
        //            if (idSize.HasValue)
        //            {
        //                var size = await _context.Size.FirstOrDefaultAsync(s => s.ID_Size == idSize.Value);
        //                giaThemSize = size?.Gia ?? 0;
        //            }

        //            // Tính tổng tiền cho chi tiết hóa đơn
        //            decimal tongTienChiTiet = (item.Gia_Hien_Thi + giaThemSize) * item.So_Luong;
        //            foreach (var topping in item.HoaDonChiTietToppings)
        //            {
        //                tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;
        //            }

        //            // Cập nhật ID và tổng tiền trong DTO
        //            item.ID_Size = idSize;
        //            item.ID_SanPham_DoNgot = idDoNgot;
        //            item.ID_LuongDa = idLuongDa;
        //            item.Gia_Them_Size = giaThemSize;
        //            item.Gia_San_Pham = item.Gia_Hien_Thi;
        //            item.Tong_Tien = tongTienChiTiet;
        //            item.Ma_HoaDon_ChiTiet = Guid.NewGuid().ToString(); // Tạo mã ngẫu nhiên

        //            // Tạo chi tiết hóa đơn
        //            var hoaDonChiTiet = new HoaDonChiTiet
        //            {
        //                ID_Hoa_Don = hoaDon.ID_Hoa_Don,
        //                ID_San_Pham = item.ID_San_Pham,
        //                ID_Size = idSize,
        //                ID_SanPham_DoNgot = idDoNgot,
        //                ID_LuongDa = idLuongDa,
        //                Ma_HoaDon_ChiTiet = item.Ma_HoaDon_ChiTiet,
        //                Gia_Them_Size = giaThemSize,
        //                Gia_San_Pham = item.Gia_Hien_Thi,
        //                Tong_Tien = tongTienChiTiet,
        //                So_Luong = item.So_Luong,
        //                Ghi_Chu = item.Ghi_Chu,
        //                Ngay_Tao = DateTime.Now
        //            };

        //            _context.HoaDon_ChiTiet.Add(hoaDonChiTiet);
        //            await _context.SaveChangesAsync(); // Lưu để có ID_HoaDon_ChiTiet

        //            // Xử lý topping
        //            if (item.HoaDonChiTietToppings != null && item.HoaDonChiTietToppings.Any())
        //            {
        //                emailChiTietSanPham.AppendLine("Topping:");
        //                foreach (var toppingDto in item.HoaDonChiTietToppings)
        //                {
        //                    var topping = await _context.Topping
        //                        .FirstOrDefaultAsync(t => t.ID_Topping == toppingDto.ID_Topping && t.Trang_Thai == true);
        //                    if (topping == null)
        //                        throw new Exception($"Topping {toppingDto.Ten_Topping} không tồn tại hoặc không hoạt động.");

        //                    var hoaDonChiTietTopping = new HoaDonChiTietTopping
        //                    {
        //                        ID_HoaDon_ChiTiet = hoaDonChiTiet.ID_HoaDon_ChiTiet,
        //                        ID_Topping = toppingDto.ID_Topping,
        //                        So_Luong = toppingDto.So_Luong,
        //                        Gia_Topping = toppingDto.Gia_Topping
        //                    };

        //                    _context.HoaDonChiTiet_Topping.Add(hoaDonChiTietTopping);

        //                    // Thêm thông tin topping vào email
        //                    emailChiTietSanPham.AppendLine($" - {topping.Ten}: {toppingDto.So_Luong} x {toppingDto.Gia_Topping.ToString("C", _vnCulture)}");
        //                }
        //            }

        //            // Thêm thông tin sản phẩm vào email
        //            emailChiTietSanPham.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
        //            emailChiTietSanPham.AppendLine($"Số lượng: {item.So_Luong}");
        //            emailChiTietSanPham.AppendLine($"Giá sản phẩm: {item.Gia_Hien_Thi.ToString("C", _vnCulture)}");
        //            if (giaThemSize > 0)
        //                emailChiTietSanPham.AppendLine($"Giá thêm size: {giaThemSize.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine($"Tổng tiền: {tongTienChiTiet.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine("-------------------");
        //        }

        //        await _context.SaveChangesAsync();

        //        // Gửi email thông báo hóa đơn
        //        if (hoaDon.ID_Khach_Hang.HasValue)
        //        {
        //            var khachHang = await _context.Khach_Hang
        //                .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang.Value);
        //            if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
        //            {
        //                string linkTraCuu = $"https://yourdomain.com/hoa-don/tra-cuu/{hoaDon.Ma_Hoa_Don}";

        //                var emailBody = new StringBuilder();
        //                emailBody.AppendLine("Hóa đơn của bạn đã được tạo thành công!");
        //                emailBody.AppendLine($"Mã hóa đơn: {hoaDon.Ma_Hoa_Don}");
        //                emailBody.AppendLine($"Ngày tạo: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
        //                emailBody.AppendLine($"Trạng thái: {hoaDon.Trang_Thai}");
        //                emailBody.AppendLine($"Tra cứu chi tiết tại: {linkTraCuu}");
        //                emailBody.AppendLine("Cảm ơn bạn đã mua hàng!");

        //                await _emailService.SendEmailAsync(khachHang.Email, "Thông tin hóa đơn", emailBody.ToString());
        //            }
        //        }

        //        await transaction.CommitAsync();

        //        return hoaDonDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Lỗi khi xử lý thanh toán: {ex.Message}");
        //    }
        //}



        //đoạn code dưới đây thực hiện khi đơn hàng nếu : chưa thanh toán thì lập tức vẫn trừ số lượng sản phẩm vì nếu như 2 người mua thanh toán vnpay cùng 1 lúc thì 
        // sẽ gây ra âm số lượng sản phẩm trong kho

        public async Task<HoaDonBanHangTKDTO> CheckOutTk(HoaDonBanHangTKDTO hoaDonDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Kiểm tra dữ liệu giỏ hàng
                if (hoaDonDto.HoaDonChiTiets == null || !hoaDonDto.HoaDonChiTiets.Any())
                    throw new Exception("Dữ liệu giỏ hàng không hợp lệ hoặc rỗng.");

                // Kiểm tra và áp dụng voucher nếu có
                decimal giamGiaVoucher = 0;
                Voucher voucher = null;
                KhachHangVoucher khachHangVoucher = null;
                if (hoaDonDto.ID_Voucher.HasValue)
                {
                    if (!hoaDonDto.ID_Khach_Hang.HasValue)
                        throw new Exception("Phải cung cấp ID_Khach_Hang khi sử dụng voucher.");

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

                    khachHangVoucher = await _context.KhachHang_Voucher
                        .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
                                                 && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
                                                 && khv.Trang_Thai == true);
                    if (khachHangVoucher == null)
                        throw new Exception($"Khách hàng {hoaDonDto.ID_Khach_Hang} không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");

                    giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;
                    khachHangVoucher.Trang_Thai = false;
                }

                // Tạo hóa đơn
                var hoaDon = new HoaDon
                {
                    ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
                    ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
                    ID_Dia_Chi = hoaDonDto.ID_Dia_Chi,
                    ID_Phi_Ship = null,
                    Phi_Ship = 0,
                    Ngay_Tao = DateTime.Now,
                    Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher,
                    Ghi_Chu = hoaDonDto.Ghi_Chu,
                    Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
                    Loai_Hoa_Don = "Online",
                    Trang_Thai = hoaDonDto.Ghi_Chu == "VNPAY" ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
                };

                _context.Hoa_Don.Add(hoaDon);
                await _context.SaveChangesAsync();

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

                var emailChiTietSanPham = new StringBuilder();
                foreach (var item in hoaDonDto.HoaDonChiTiets)
                {
                    var sanPham = await _context.San_Pham
                        .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);
                    if (sanPham == null)
                        throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc không hoạt động.");

                    // Nếu trạng thái hóa đơn là "Chua_Thanh_Toan" thì trừ tồn kho
                    if (hoaDon.Trang_Thai == "Chua_Thanh_Toan")
                    {
                        if (sanPham.So_Luong < item.So_Luong)
                            throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ tồn kho.");

                        sanPham.So_Luong -= item.So_Luong;
                        _context.San_Pham.Update(sanPham);
                    }

                    int? idSize = null;
                    if (!string.IsNullOrEmpty(item.Ten_Size))
                    {
                        var size = await _context.Size.FirstOrDefaultAsync(s => s.SizeName == item.Ten_Size);
                        if (size == null)
                            throw new Exception($"Size {item.Ten_Size} không tồn tại.");
                        idSize = size.ID_Size;
                    }

                    int? idLuongDa = null;
                    if (!string.IsNullOrEmpty(item.Ten_LuongDa))
                    {
                        var luongDa = await _context.LuongDa.FirstOrDefaultAsync(ld => ld.Ten_LuongDa == item.Ten_LuongDa);
                        if (luongDa == null)
                            throw new Exception($"Lượng đá {item.Ten_LuongDa} không tồn tại.");
                        idLuongDa = luongDa.ID_LuongDa;
                    }

                    int? idDoNgot = null;
                    if (!string.IsNullOrEmpty(item.Ten_DoNgot))
                    {
                        var doNgot = await _context.DoNgot.FirstOrDefaultAsync(dn => dn.Muc_Do == item.Ten_DoNgot);
                        if (doNgot == null)
                            throw new Exception($"Độ ngọt {item.Ten_DoNgot} không tồn tại.");
                        idDoNgot = doNgot.ID_DoNgot;
                    }

                    decimal giaThemSize = 0;
                    if (idSize.HasValue)
                    {
                        var size = await _context.Size.FirstOrDefaultAsync(s => s.ID_Size == idSize.Value);
                        giaThemSize = size?.Gia ?? 0;
                    }

                    decimal tongTienChiTiet = (item.Gia_Hien_Thi + giaThemSize) * item.So_Luong;
                    foreach (var topping in item.HoaDonChiTietToppings)
                    {
                        tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;
                    }

                    item.ID_Size = idSize;
                    item.ID_SanPham_DoNgot = idDoNgot;
                    item.ID_LuongDa = idLuongDa;
                    item.Gia_Them_Size = giaThemSize;
                    item.Gia_San_Pham = item.Gia_Hien_Thi;
                    item.Tong_Tien = tongTienChiTiet;
                    item.Ma_HoaDon_ChiTiet = Guid.NewGuid().ToString();

                    var hoaDonChiTiet = new HoaDonChiTiet
                    {
                        ID_Hoa_Don = hoaDon.ID_Hoa_Don,
                        ID_San_Pham = item.ID_San_Pham,
                        ID_Size = idSize,
                        ID_SanPham_DoNgot = idDoNgot,
                        ID_LuongDa = idLuongDa,
                        Ma_HoaDon_ChiTiet = item.Ma_HoaDon_ChiTiet,
                        Gia_Them_Size = giaThemSize,
                        Gia_San_Pham = item.Gia_Hien_Thi,
                        Tong_Tien = tongTienChiTiet,
                        So_Luong = item.So_Luong,
                        Ghi_Chu = item.Ghi_Chu,
                        Ngay_Tao = DateTime.Now
                    };

                    _context.HoaDon_ChiTiet.Add(hoaDonChiTiet);
                    await _context.SaveChangesAsync();

                    if (item.HoaDonChiTietToppings != null && item.HoaDonChiTietToppings.Any())
                    {
                        emailChiTietSanPham.AppendLine("Topping:");
                        foreach (var toppingDto in item.HoaDonChiTietToppings)
                        {
                            var topping = await _context.Topping
                                .FirstOrDefaultAsync(t => t.ID_Topping == toppingDto.ID_Topping && t.Trang_Thai == true);
                            if (topping == null)
                                throw new Exception($"Topping {toppingDto.Ten_Topping} không tồn tại hoặc không hoạt động.");

                            var hoaDonChiTietTopping = new HoaDonChiTietTopping
                            {
                                ID_HoaDon_ChiTiet = hoaDonChiTiet.ID_HoaDon_ChiTiet,
                                ID_Topping = toppingDto.ID_Topping,
                                So_Luong = toppingDto.So_Luong,
                                Gia_Topping = toppingDto.Gia_Topping
                            };

                            _context.HoaDonChiTiet_Topping.Add(hoaDonChiTietTopping);
                            emailChiTietSanPham.AppendLine($" - {topping.Ten}: {toppingDto.So_Luong} x {toppingDto.Gia_Topping.ToString("C", _vnCulture)}");
                        }
                    }

                    emailChiTietSanPham.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
                    emailChiTietSanPham.AppendLine($"Số lượng: {item.So_Luong}");
                    emailChiTietSanPham.AppendLine($"Giá sản phẩm: {item.Gia_Hien_Thi.ToString("C", _vnCulture)}");
                    if (giaThemSize > 0)
                        emailChiTietSanPham.AppendLine($"Giá thêm size: {giaThemSize.ToString("C", _vnCulture)}");
                    emailChiTietSanPham.AppendLine($"Tổng tiền: {tongTienChiTiet.ToString("C", _vnCulture)}");
                    emailChiTietSanPham.AppendLine("-------------------");
                }

                await _context.SaveChangesAsync();

                if (hoaDon.ID_Khach_Hang.HasValue)
                {
                    var khachHang = await _context.Khach_Hang
                        .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang.Value);
                    if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
                    {
                        string linkTraCuu = $"https://yourdomain.com/hoa-don/tra-cuu/{hoaDon.Ma_Hoa_Don}";

                        var emailBody = new StringBuilder();
                        emailBody.AppendLine("Hóa đơn của bạn đã được tạo thành công!");
                        emailBody.AppendLine($"Mã hóa đơn: {hoaDon.Ma_Hoa_Don}");
                        emailBody.AppendLine($"Ngày tạo: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
                        emailBody.AppendLine($"Trạng thái: {hoaDon.Trang_Thai}");
                        emailBody.AppendLine($"Tra cứu chi tiết tại: {linkTraCuu}");
                        emailBody.AppendLine("Cảm ơn bạn đã mua hàng!");

                        await _emailService.SendEmailAsync(khachHang.Email, "Thông tin hóa đơn", emailBody.ToString());
                    }
                }

                await transaction.CommitAsync();

                return hoaDonDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi khi xử lý thanh toán: {ex.Message}");
            }
        }




        public async Task<bool> KiemTraCoSoDienThoaiAsync(int idKhachHang)
        {
            var khachHang = await _context.Khach_Hang.FindAsync(idKhachHang);
            if (khachHang == null)
            {
                return false; // Hoặc throw exception nếu cần
            }
            return !string.IsNullOrEmpty(khachHang.So_Dien_Thoai);
        }

        public async Task<string> LaySoDienThoaiAsync(int idKhachHang)
        {
            var khachHang = await _context.Khach_Hang
                .Where(k => k.ID_Khach_Hang == idKhachHang)
                .Select(k => k.So_Dien_Thoai)
                .FirstOrDefaultAsync();
            return khachHang;
        }
        public async Task ThemSoDienThoaiAsync(int idKhachHang, string soDienThoai)
        {
            var khachHang = await _context.Khach_Hang.FindAsync(idKhachHang);
            if (khachHang == null)
            {
                // Throw exception hoặc xử lý lỗi
                throw new Exception("Khách hàng không tồn tại");
            }
            if (!string.IsNullOrEmpty(khachHang.So_Dien_Thoai))
            {
                // Có thể throw nếu đã có, nhưng theo yêu cầu API 2 là thêm (giả sử thêm nếu chưa có)
                throw new Exception("Khách hàng đã có số điện thoại");
            }
            khachHang.So_Dien_Thoai = soDienThoai;
            await _context.SaveChangesAsync();
        }

        public async Task CapNhatSoDienThoaiAsync(int idKhachHang, string soDienThoaiMoi)
        {
            var khachHang = await _context.Khach_Hang.FindAsync(idKhachHang);
            if (khachHang == null)
            {
                // Throw exception hoặc xử lý lỗi
                throw new Exception("Khách hàng không tồn tại");
            }
            khachHang.So_Dien_Thoai = soDienThoaiMoi;
            await _context.SaveChangesAsync();
        }



    }

}
