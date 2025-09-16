using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using System.Globalization;
using System.Text;
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




        public async Task<HoaDonBanHangCKDTO> CheckOutTk(HoaDonBanHangCKDTO hoaDonDto)
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
                            throw new Exception($"Khách hàng {hoaDonDto.ID_Khach_Hang} không sở hữu voucher {voucher.Ma_Voucher} hoặc voucher đã sử dụng.");
                    }
                    giamGiaVoucher = voucher.Gia_Tri_Giam ?? 0;
                    if (khachHangVoucher != null)
                        khachHangVoucher.Trang_Thai = false;
                }

                // Tạo hóa đơn
                var hoaDon = new HoaDon
                {
                    ID_Khach_Hang = hoaDonDto.ID_Khach_Hang,
                    ID_Hinh_Thuc_Thanh_Toan = hoaDonDto.ID_Hinh_Thuc_Thanh_Toan,
                    Dia_Chi_Tu_Nhap = hoaDonDto.Dia_Chi_Tu_Nhap,
                    ID_Phi_Ship = null,
                    Phi_Ship = 0,
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
