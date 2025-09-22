using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly ILogger<BanHangTKRepository> _logger; // Khai báo ILogger

        public BanHangTKRepository(MyDbContext context , ILogger<BanHangTKRepository> logger, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger; // Khởi tạo ILogger trong constructor
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





        //đoạn code dưới đây thực hiện khi đơn hàng nếu : chưa thanh toán thì lập tức vẫn trừ số lượng sản phẩm vì nếu như 2 người mua thanh toán vnpay cùng 1 lúc thì 
        // sẽ gây ra âm số lượng sản phẩm trong kho
        // thực hiện check id phương thức thanh toán nếu là id 5 vnpay thì trừ kho luôn



        //Fix lỗi khi áp dụng voucher 
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
        //            voucher = await _context.Voucher
        //                .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                       && v.Trang_Thai == true
        //                                       && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));
        //            if (voucher == null)
        //                throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

        //            // Bỏ kiểm tra tổng tiền tối thiểu
        //            // if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && hoaDonDto.Tong_Tien < voucher.So_Tien_Dat_Yeu_Cau.Value)
        //            //     throw new Exception($"Tổng tiền hóa đơn không đủ để sử dụng voucher {voucher.Ma_Voucher}.");

        //            khachHangVoucher = await _context.KhachHang_Voucher
        //                .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
        //                                         && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                         && khv.Trang_Thai == true);
        //            if (khachHangVoucher == null)
        //                throw new Exception($"Khách hàng {hoaDonDto.ID_Khach_Hang} không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");

        //            giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;
        //            khachHangVoucher.Trang_Thai = false;
        //        }

        //        // Tạo hóa đơn
        //        var hoaDon = new HoaDon
        //        {
        //            ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
        //            ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
        //            ID_Dia_Chi = hoaDonDto.ID_Dia_Chi,
        //            ID_Phi_Ship = null,
        //            Phi_Ship = hoaDonDto.Phi_Ship,
        //            Ngay_Tao = DateTime.Now,
        //            Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher,
        //            Ghi_Chu = hoaDonDto.Ghi_Chu,
        //            Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
        //            Loai_Hoa_Don = "Online",
        //            Trang_Thai = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan == 5 ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
        //        };
        //        _context.Hoa_Don.Add(hoaDon);
        //        await _context.SaveChangesAsync();

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

        //        var emailChiTietSanPham = new StringBuilder();
        //        foreach (var item in hoaDonDto.HoaDonChiTiets)
        //        {
        //            var sanPham = await _context.San_Pham
        //                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);
        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc không hoạt động.");

        //            // Nếu trạng thái hóa đơn là "Chua_Thanh_Toan" thì trừ tồn kho
        //            if (hoaDon.Trang_Thai == "Chua_Thanh_Toan")
        //            {
        //                if (sanPham.So_Luong < item.So_Luong)
        //                    throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ tồn kho.");
        //                sanPham.So_Luong -= item.So_Luong;
        //                _context.San_Pham.Update(sanPham);
        //            }

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

        //            decimal giaThemSize = 0;
        //            if (idSize.HasValue)
        //            {
        //                var size = await _context.Size.FirstOrDefaultAsync(s => s.ID_Size == idSize.Value);
        //                giaThemSize = size?.Gia ?? 0;
        //            }

        //            decimal tongTienChiTiet = (item.Gia_Hien_Thi + giaThemSize) * item.So_Luong;
        //            foreach (var topping in item.HoaDonChiTietToppings)
        //            {
        //                tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;
        //            }

        //            item.ID_Size = idSize;
        //            item.ID_SanPham_DoNgot = idDoNgot;
        //            item.ID_LuongDa = idLuongDa;
        //            item.Gia_Them_Size = giaThemSize;
        //            item.Gia_San_Pham = item.Gia_Hien_Thi;
        //            item.Tong_Tien = tongTienChiTiet;
        //            item.Ma_HoaDon_ChiTiet = Guid.NewGuid().ToString();

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
        //            await _context.SaveChangesAsync();

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
        //                    emailChiTietSanPham.AppendLine($" - {topping.Ten}: {toppingDto.So_Luong} x {toppingDto.Gia_Topping.ToString("C", _vnCulture)}");
        //                }
        //            }

        //            emailChiTietSanPham.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
        //            emailChiTietSanPham.AppendLine($"Số lượng: {item.So_Luong}");
        //            emailChiTietSanPham.AppendLine($"Giá sản phẩm: {item.Gia_Hien_Thi.ToString("C", _vnCulture)}");
        //            if (giaThemSize > 0)
        //                emailChiTietSanPham.AppendLine($"Giá thêm size: {giaThemSize.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine($"Tổng tiền: {tongTienChiTiet.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine("-------------------");
        //        }

        //        await _context.SaveChangesAsync();

        //        if (hoaDon.ID_Khach_Hang.HasValue)
        //        {
        //            var khachHang = await _context.Khach_Hang
        //                .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang.Value);
        //            if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
        //            {
        //                var emailBody = new StringBuilder();
        //                emailBody.AppendLine("Hóa đơn của bạn đã được tạo thành công!");
        //                emailBody.AppendLine($"Mã hóa đơn: {hoaDon.Ma_Hoa_Don}");
        //                emailBody.AppendLine($"Ngày tạo: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
        //                emailBody.AppendLine($"Trạng thái: {hoaDon.Trang_Thai}");
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


        //đoạn code dười kkhi đơn hàng Chua_Xac_Nhan thì  trừ số lượng của sản phẩm
        // vì khi ví dụ còn 1 sản phầm mà 1000 khách đặt thì chỉ có 1 khách đặt đc những khách kia không đặt được , không gây quá tải khi nhân viên hủy đơn hàng


        //form gửi gmail cũng hơi xấu 

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
        //            voucher = await _context.Voucher
        //                .FirstOrDefaultAsync(v => v.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                       && v.Trang_Thai == true
        //                                       && (v.Ngay_Ket_Thuc == null || v.Ngay_Ket_Thuc >= currentTime));
        //            if (voucher == null)
        //                throw new Exception($"Voucher {hoaDonDto.ID_Voucher} không tồn tại hoặc không hợp lệ.");

        //            khachHangVoucher = await _context.KhachHang_Voucher
        //                .FirstOrDefaultAsync(khv => khv.ID_Khach_Hang == hoaDonDto.ID_Khach_Hang.Value
        //                                         && khv.ID_Voucher == hoaDonDto.ID_Voucher.Value
        //                                         && khv.Trang_Thai == true);
        //            if (khachHangVoucher == null)
        //                throw new Exception($"Khách hàng {hoaDonDto.ID_Khach_Hang} không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");

        //            giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;
        //            khachHangVoucher.Trang_Thai = false;
        //        }

        //        // Tạo hóa đơn
        //        var hoaDon = new HoaDon
        //        {
        //            ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
        //            ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
        //            ID_Dia_Chi = hoaDonDto.ID_Dia_Chi,
        //            ID_Phi_Ship = null,
        //            Phi_Ship = hoaDonDto.Phi_Ship,
        //            Ngay_Tao = DateTime.Now,
        //            Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher,
        //            Ghi_Chu = hoaDonDto.Ghi_Chu,
        //            Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
        //            Loai_Hoa_Don = "Online",
        //            Trang_Thai = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan == 5 ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
        //        };
        //        _context.Hoa_Don.Add(hoaDon);
        //        await _context.SaveChangesAsync();

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

        //        var emailChiTietSanPham = new StringBuilder();
        //        foreach (var item in hoaDonDto.HoaDonChiTiets)
        //        {
        //            var sanPham = await _context.San_Pham
        //                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);
        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc không hoạt động.");

        //            // Trừ tồn kho sản phẩm nếu trạng thái là "Chua_Thanh_Toan" hoặc "Chua_Xac_Nhan"
        //            if (hoaDon.Trang_Thai == "Chua_Thanh_Toan" || hoaDon.Trang_Thai == "Chua_Xac_Nhan")
        //            {
        //                if (sanPham.So_Luong < item.So_Luong)
        //                    throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ tồn kho.");
        //                sanPham.So_Luong -= item.So_Luong;
        //                _context.San_Pham.Update(sanPham);
        //            }

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

        //            decimal giaThemSize = 0;
        //            if (idSize.HasValue)
        //            {
        //                var size = await _context.Size.FirstOrDefaultAsync(s => s.ID_Size == idSize.Value);
        //                giaThemSize = size?.Gia ?? 0;
        //            }

        //            decimal tongTienChiTiet = (item.Gia_Hien_Thi + giaThemSize) * item.So_Luong;
        //            foreach (var topping in item.HoaDonChiTietToppings)
        //            {
        //                tongTienChiTiet += topping.Gia_Topping * topping.So_Luong;
        //            }

        //            item.ID_Size = idSize;
        //            item.ID_SanPham_DoNgot = idDoNgot;
        //            item.ID_LuongDa = idLuongDa;
        //            item.Gia_Them_Size = giaThemSize;
        //            item.Gia_San_Pham = item.Gia_Hien_Thi;
        //            item.Tong_Tien = tongTienChiTiet;
        //            item.Ma_HoaDon_ChiTiet = Guid.NewGuid().ToString();

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
        //            await _context.SaveChangesAsync();

        //            if (item.HoaDonChiTietToppings != null && item.HoaDonChiTietToppings.Any())
        //            {
        //                emailChiTietSanPham.AppendLine("Topping:");
        //                foreach (var toppingDto in item.HoaDonChiTietToppings)
        //                {
        //                    var topping = await _context.Topping
        //                        .FirstOrDefaultAsync(t => t.ID_Topping == toppingDto.ID_Topping && t.Trang_Thai == true);
        //                    if (topping == null)
        //                        throw new Exception($"Topping {toppingDto.Ten_Topping} không tồn tại hoặc không hoạt động.");

        //                    // Trừ tồn kho topping nếu trạng thái là "Chua_Thanh_Toan" hoặc "Chua_Xac_Nhan"
        //                    if (hoaDon.Trang_Thai == "Chua_Thanh_Toan" || hoaDon.Trang_Thai == "Chua_Xac_Nhan")
        //                    {
        //                        if (topping.So_Luong < toppingDto.So_Luong)
        //                            throw new Exception($"Topping {topping.Ten} không đủ tồn kho.");
        //                        topping.So_Luong -= toppingDto.So_Luong;
        //                        _context.Topping.Update(topping);
        //                    }

        //                    var hoaDonChiTietTopping = new HoaDonChiTietTopping
        //                    {
        //                        ID_HoaDon_ChiTiet = hoaDonChiTiet.ID_HoaDon_ChiTiet,
        //                        ID_Topping = toppingDto.ID_Topping,
        //                        So_Luong = toppingDto.So_Luong,
        //                        Gia_Topping = toppingDto.Gia_Topping
        //                    };
        //                    _context.HoaDonChiTiet_Topping.Add(hoaDonChiTietTopping);
        //                    emailChiTietSanPham.AppendLine($" - {topping.Ten}: {toppingDto.So_Luong} x {toppingDto.Gia_Topping.ToString("C", _vnCulture)}");
        //                }
        //            }

        //            emailChiTietSanPham.AppendLine($"Sản phẩm: {sanPham.Ten_San_Pham}");
        //            emailChiTietSanPham.AppendLine($"Số lượng: {item.So_Luong}");
        //            emailChiTietSanPham.AppendLine($"Giá sản phẩm: {item.Gia_Hien_Thi.ToString("C", _vnCulture)}");
        //            if (giaThemSize > 0)
        //                emailChiTietSanPham.AppendLine($"Giá thêm size: {giaThemSize.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine($"Tổng tiền: {tongTienChiTiet.ToString("C", _vnCulture)}");
        //            emailChiTietSanPham.AppendLine("-------------------");
        //        }

        //        await _context.SaveChangesAsync();

        //        if (hoaDon.ID_Khach_Hang.HasValue)
        //        {
        //            var khachHang = await _context.Khach_Hang
        //                .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang.Value);
        //            if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
        //            {
        //                var emailBody = new StringBuilder();
        //                emailBody.AppendLine("Hóa đơn của bạn đã được tạo thành công!");
        //                emailBody.AppendLine($"Mã hóa đơn: {hoaDon.Ma_Hoa_Don}");
        //                emailBody.AppendLine($"Ngày tạo: {hoaDon.Ngay_Tao:dd/MM/yyyy HH:mm}");
        //                emailBody.AppendLine($"Trạng thái: {hoaDon.Trang_Thai}");
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



        //HIỂN THỊ GMAILĐẸO HƠN 



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
                    Phi_Ship = hoaDonDto.Phi_Ship,
                    Ngay_Tao = DateTime.Now,
                    Tong_Tien = hoaDonDto.Tong_Tien - giamGiaVoucher,
                    Ghi_Chu = hoaDonDto.Ghi_Chu,
                    Ma_Hoa_Don = hoaDonDto.Ma_Hoa_Don,
                    Loai_Hoa_Don = "Online",
                    Trang_Thai = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan == 5 ? "Chua_Thanh_Toan" : "Chua_Xac_Nhan"
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

                // Xây dựng nội dung email HTML
                var emailBody = new StringBuilder();
                emailBody.AppendLine(@"<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body { font-family: Arial, sans-serif; color: #333; line-height: 1.6; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px; }
        .header { background-color: #f8f8f8; padding: 15px; text-align: center; border-radius: 8px 8px 0 0; }
        .header h1 { margin: 0; font-size: 24px; color: #333; }
        .order-details { margin: 20px 0; }
        .order-details h2 { font-size: 18px; color: #333; }
        .item { border-bottom: 1px solid #e0e0e0; padding: 10px 0; }
        .item:last-child { border-bottom: none; }
        .item p { margin: 5px 0; }
        .summary { margin-top: 20px; padding-top: 10px; border-top: 1px solid #e0e0e0; }
        .summary p { font-size: 16px; font-weight: bold; }
        .footer { text-align: center; margin-top: 20px; color: #777; font-size: 12px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Xác nhận đơn hàng #" + hoaDon.Ma_Hoa_Don + @"</h1>
            <p>Ngày đặt: " + hoaDon.Ngay_Tao.ToString("dd/MM/yyyy HH:mm") + @"</p>
            <p>Trạng thái: " + hoaDon.Trang_Thai + @"</p>
        </div>
        <div class='order-details'>
            <h2>Chi tiết đơn hàng</h2>");

                foreach (var item in hoaDonDto.HoaDonChiTiets)
                {
                    var sanPham = await _context.San_Pham
                        .FirstOrDefaultAsync(sp => sp.ID_San_Pham == item.ID_San_Pham && sp.Trang_Thai == true);
                    if (sanPham == null)
                        throw new Exception($"Sản phẩm {item.Ten_San_Pham} không tồn tại hoặc không hoạt động.");

                    // Trừ tồn kho sản phẩm nếu trạng thái là "Chua_Thanh_Toan" hoặc "Chua_Xac_Nhan"
                    if (hoaDon.Trang_Thai == "Chua_Thanh_Toan" || hoaDon.Trang_Thai == "Chua_Xac_Nhan")
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

                    // Thêm chi tiết sản phẩm vào email HTML
                    emailBody.AppendLine($@"<div class='item'>
                <p><strong>Sản phẩm:</strong> {sanPham.Ten_San_Pham}</p>
                <p><strong>Số lượng:</strong> {item.So_Luong}</p>
                <p><strong>Giá sản phẩm:</strong> {item.Gia_Hien_Thi:N0} đ</p>");
                    if (giaThemSize > 0)
                        emailBody.AppendLine($"<p><strong>Giá thêm size:</strong> {giaThemSize:N0} đ</p>");
                    if (!string.IsNullOrEmpty(item.Ten_Size))
                        emailBody.AppendLine($"<p><strong>Size:</strong> {item.Ten_Size}</p>");
                    if (!string.IsNullOrEmpty(item.Ten_LuongDa))
                        emailBody.AppendLine($"<p><strong>Lượng đá:</strong> {item.Ten_LuongDa}</p>");
                    if (!string.IsNullOrEmpty(item.Ten_DoNgot))
                        emailBody.AppendLine($"<p><strong>Độ ngọt:</strong> {item.Ten_DoNgot}</p>");
                    if (item.HoaDonChiTietToppings != null && item.HoaDonChiTietToppings.Any())
                    {
                        emailBody.AppendLine("<p><strong>Topping:</strong></p><ul>");
                        foreach (var toppingDto in item.HoaDonChiTietToppings)
                        {
                            var topping = await _context.Topping
                                .FirstOrDefaultAsync(t => t.ID_Topping == toppingDto.ID_Topping && t.Trang_Thai == true);
                            if (topping == null)
                                throw new Exception($"Topping {toppingDto.Ten_Topping} không tồn tại hoặc không hoạt động.");

                            // Trừ tồn kho topping nếu trạng thái là "Chua_Thanh_Toan" hoặc "Chua_Xac_Nhan"
                            if (hoaDon.Trang_Thai == "Chua_Thanh_Toan" || hoaDon.Trang_Thai == "Chua_Xac_Nhan")
                            {
                                if (topping.So_Luong < toppingDto.So_Luong)
                                    throw new Exception($"Topping {topping.Ten} không đủ tồn kho.");
                                topping.So_Luong -= toppingDto.So_Luong;
                                _context.Topping.Update(topping);
                            }

                            var hoaDonChiTietTopping = new HoaDonChiTietTopping
                            {
                                ID_HoaDon_ChiTiet = hoaDonChiTiet.ID_HoaDon_ChiTiet,
                                ID_Topping = toppingDto.ID_Topping,
                                So_Luong = toppingDto.So_Luong,
                                Gia_Topping = toppingDto.Gia_Topping
                            };
                            _context.HoaDonChiTiet_Topping.Add(hoaDonChiTietTopping);
                            emailBody.AppendLine($"<li>{topping.Ten} (x{toppingDto.So_Luong}): {toppingDto.Gia_Topping * toppingDto.So_Luong:N0} đ</li>");
                        }
                        emailBody.AppendLine("</ul>");
                    }
                    emailBody.AppendLine($"<p><strong>Tổng tiền:</strong> {tongTienChiTiet:N0} đ</p>");
                    emailBody.AppendLine("</div>");
                }

                // Tổng tiền hiển thị cho khách = DB.Tong_Tien + Phi_Ship - Voucher
                decimal tongTienHienThi = (decimal)hoaDon.Tong_Tien + (decimal)hoaDon.Phi_Ship;
                emailBody.AppendLine($@"<div class='summary'>
            <p>Phí ship: {hoaDon.Phi_Ship:N0} đ</p>
            {(voucher != null ? $"<p>Giảm giá voucher ({voucher.Ma_Voucher}): -{giamGiaVoucher:N0} đ</p>" : "")}
            <p>Thành tiền: {tongTienHienThi - giamGiaVoucher:N0} đ</p>
        </div>
        <div class='footer'>
            <p>Cảm ơn bạn đã mua sắm tại cửa hàng của chúng tôi!</p>
            <p>Nếu có bất kỳ câu hỏi nào, vui lòng liên hệ qua email hoặc hotline.</p>
        </div>
    </div>
</body>
</html>");

                await _context.SaveChangesAsync();

                // Gửi email xác nhận
                if (hoaDon.ID_Khach_Hang.HasValue)
                {
                    var khachHang = await _context.Khach_Hang
                        .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == hoaDon.ID_Khach_Hang.Value);
                    if (khachHang != null && !string.IsNullOrEmpty(khachHang.Email))
                    {
                        await _emailService.SendEmailAsync(
                            khachHang.Email,
                            $"Xác nhận đơn hàng #{hoaDon.Ma_Hoa_Don}",
                            emailBody.ToString()
                           
                        );
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


        //Phương thức cập nhật cá nhân khách hàng trong trang checkout 


        public async Task<KhachHangCheckoutDTO> GetKhachHangByIdAsync(int id)
        {
            var khachHang = await _context.Khach_Hang
                .Where(kh => kh.ID_Khach_Hang == id)
                .Select(kh => new KhachHangCheckoutDTO
                {
                    ID_Khach_Hang = kh.ID_Khach_Hang,
                    Ho_Ten = kh.Ho_Ten,
                    Email = kh.Email,
                    So_Dien_Thoai = kh.So_Dien_Thoai
                })
                .FirstOrDefaultAsync();

            return khachHang;
        }

        //public async Task<bool> UpdateKhachHangAsync(int id, KhachHangCheckoutDTO khachHangDto)
        //{
        //    var khachHang = await _context.Khach_Hang
        //        .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == id);

        //    if (khachHang == null)
        //    {
        //        return false;
        //    }

        //    khachHang.Ho_Ten = khachHangDto.Ho_Ten;
        //    khachHang.Email = khachHangDto.Email;
        //    khachHang.So_Dien_Thoai = khachHangDto.So_Dien_Thoai;

        //    _context.Khach_Hang.Update(khachHang);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}



        public async Task<bool> UpdateKhachHangAsync(int id, KhachHangCheckoutDTO khachHangDto)
        {
            // Tìm khách hàng theo ID
            var khachHang = await _context.Khach_Hang
                .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == id);

            if (khachHang == null)
            {
                _logger.LogWarning($"Không tìm thấy khách hàng với ID {id}");
                return false;
            }

            // Kiểm tra email trùng lặp (ngoại trừ chính khách hàng đang cập nhật)
            if (!string.IsNullOrWhiteSpace(khachHangDto.Email))
            {
                // So sánh không phân biệt chữ hoa/thường
                var emailExists = await _context.Khach_Hang
                    .AnyAsync(kh => kh.Email.ToLower() == khachHangDto.Email.ToLower() && kh.ID_Khach_Hang != id);

                if (emailExists)
                {
                    _logger.LogWarning($"Email {khachHangDto.Email} đã được sử dụng bởi khách hàng khác (ID != {id})");
                    throw new InvalidOperationException("Email đã được sử dụng bởi một khách hàng khác.");
                }
            }

            // Cập nhật thông tin khách hàng
            khachHang.Ho_Ten = khachHangDto.Ho_Ten;
            khachHang.Email = khachHangDto.Email;
            khachHang.So_Dien_Thoai = khachHangDto.So_Dien_Thoai;

            _context.Khach_Hang.Update(khachHang);
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Cập nhật thành công khách hàng ID {id} với email {khachHangDto.Email}");
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Lỗi khi lưu cập nhật khách hàng ID {id}: {ex.Message}");
                throw new InvalidOperationException("Lỗi khi cập nhật khách hàng. Vui lòng kiểm tra dữ liệu đầu vào.");
            }
        }


    }

}
