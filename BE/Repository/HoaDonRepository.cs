using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BE.Repository
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly MyDbContext _context;
        private readonly EmailService _emailService;

        public HoaDonRepository(MyDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ===== DANH SÁCH DẠNG DTO (cho UI list) =====
        public async Task<IEnumerable<HoaDonDTO>> GetAllAsync()
        {
            return await _context.Hoa_Don
                .Include(h => h.HinhThucThanhToan)
                .Select(h => new HoaDonDTO
                {
                    ID_Hoa_Don = h.ID_Hoa_Don,
                    Ma_Hoa_Don = h.Ma_Hoa_Don,
                    Ngay_Tao = h.Ngay_Tao,
                    Tong_Tien = h.Tong_Tien,
                    Trang_Thai = h.Trang_Thai,
                    Loai_Hoa_Don = h.Loai_Hoa_Don,
                    ID_Hinh_Thuc_Thanh_Toan = h.ID_Hinh_Thuc_Thanh_Toan,
                    Ten_Hinh_Thuc_Thanh_Toan = h.HinhThucThanhToan != null
                        ? h.HinhThucThanhToan.Phuong_Thuc_Thanh_Toan
                        : null,
                    Dia_Chi_Tu_Nhap = h.Dia_Chi_Tu_Nhap
                })
                .OrderByDescending(x => x.Ngay_Tao)
                .ToListAsync();
        }

        // ===== DANH SÁCH ENTITY (giữ tương thích FE cũ) =====
        public async Task<IEnumerable<HoaDon>> GetAllEntitiesAsync()
            => await _context.Hoa_Don.AsNoTracking()
                                     .Include(x => x.KhachHang)   // ⭐ THÊM
                                     .Include(x => x.DiaChi)      // ⭐ THÊM
                                     .OrderByDescending(x => x.Ngay_Tao)
                                     .ToListAsync();

        // ===== CHI TIẾT =====
        public async Task<HoaDon> GetByIdAsync(int id)
        {
            return await _context.Hoa_Don
                .AsNoTracking()
                .Include(hd => hd.KhachHang)
                .Include(hd => hd.HinhThucThanhToan)
                .Include(hd => hd.DiaChi)
                .Include(hd => hd.HoaDonChiTiets).ThenInclude(ct => ct.SanPham)
                .Include(hd => hd.HoaDonChiTiets).ThenInclude(ct => ct.Size)
                .Include(hd => hd.HoaDonChiTiets).ThenInclude(ct => ct.DoNgot)
                .Include(hd => hd.HoaDonChiTiets).ThenInclude(ct => ct.LuongDa)
                .Include(hd => hd.HoaDonChiTiets)
                    .ThenInclude(ctt => ctt.HoaDonChiTietToppings)
                    .ThenInclude(t => t.Topping)
                .FirstOrDefaultAsync(hd => hd.ID_Hoa_Don == id);
        }

        public async Task AddAsync(HoaDon entity)
        {
            _context.Hoa_Don.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, HoaDon entity)
        {
            var existing = await _context.Hoa_Don.FirstOrDefaultAsync(x => x.ID_Hoa_Don == id);
            if (existing == null)
                throw new KeyNotFoundException($"HoaDon with ID {id} not found.");

            existing.Ma_Hoa_Don = entity.Ma_Hoa_Don;
            existing.Ngay_Tao = entity.Ngay_Tao;
            existing.Tong_Tien = entity.Tong_Tien;
            existing.Trang_Thai = entity.Trang_Thai;

            _context.Hoa_Don.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Hoa_Don.FirstOrDefaultAsync(x => x.ID_Hoa_Don == id);
            if (existing == null)
                throw new KeyNotFoundException($"HoaDon with ID {id} not found.");

            _context.Hoa_Don.Remove(existing);
            await _context.SaveChangesAsync();
        }

        // ==== UpdateTrangThaiAsync: (bản có trừ kho + email) ====
        //public async Task<bool> UpdateTrangThaiAsync(int id, string trangThai, string? lyDoHuy, EmailService emailService)
        //{
        //    var hd = await _context.Hoa_Don
        //        .Include(h => h.KhachHang)
        //        .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.SanPham)
        //        .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.HoaDonChiTietToppings).ThenInclude(hdctt => hdctt.Topping)
        //        .FirstOrDefaultAsync(x => x.ID_Hoa_Don == id);

        //    if (hd == null) return false;

        //    if (trangThai == "Da_Xac_Nhan")
        //    {
        //        foreach (var hdct in hd.HoaDonChiTiets)
        //        {
        //            var sanPham = hdct.SanPham;
        //            if (sanPham == null)
        //                throw new Exception($"Sản phẩm với ID {hdct.ID_San_Pham} không tồn tại.");
        //            if (sanPham.So_Luong < hdct.So_Luong)
        //                throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ số lượng.");

        //            sanPham.So_Luong -= hdct.So_Luong;

        //            if (hdct.HoaDonChiTietToppings != null && hdct.HoaDonChiTietToppings.Any())
        //            {
        //                foreach (var topping in hdct.HoaDonChiTietToppings)
        //                {
        //                    var toppingEntity = topping.Topping;
        //                    if (toppingEntity == null)
        //                        throw new Exception($"Topping với ID {topping.ID_Topping} không tồn tại.");
        //                    if (toppingEntity.So_Luong < topping.So_Luong)
        //                        throw new Exception($"Topping {toppingEntity.Ten} không đủ số lượng.");

        //                    toppingEntity.So_Luong -= topping.So_Luong ?? 0;
        //                }
        //            }
        //        }
        //    }

        //    hd.Trang_Thai = trangThai;
        //    if (!string.IsNullOrWhiteSpace(lyDoHuy) && trangThai == "Huy_Don")
        //        hd.LyDoHuyDon = lyDoHuy;

        //    await _context.SaveChangesAsync();

        //    if (hd.KhachHang != null && !string.IsNullOrWhiteSpace(hd.KhachHang.Email))
        //    {
        //        var subject = $"Cập nhật trạng thái hóa đơn {hd.Ma_Hoa_Don}";
        //        var productList = string.Join("\n", hd.HoaDonChiTiets.Select(hdct =>
        //        {
        //            var toppingList = hdct.HoaDonChiTietToppings != null && hdct.HoaDonChiTietToppings.Any()
        //                ? string.Join("\n", hdct.HoaDonChiTietToppings.Select(t =>
        //                    $"  - Topping: {t.Topping?.Ten} (Số lượng: {t.So_Luong}, Giá: {t.Gia_Topping})"))
        //                : "  - Không có topping";
        //            return $"- {hdct.SanPham?.Ten_San_Pham} (Số lượng: {hdct.So_Luong}, Giá: {hdct.Gia_San_Pham})\n{toppingList}";
        //        }));
        //        var body = new StringBuilder();
        //        body.AppendLine($"Kính gửi {hd.KhachHang.Ho_Ten},");
        //        body.AppendLine();
        //        body.AppendLine($"Hóa đơn {hd.Ma_Hoa_Don} của bạn đã được cập nhật trạng thái thành: {trangThai}.");
        //        body.AppendLine("Chi tiết sản phẩm và topping:");
        //        body.AppendLine(productList);
        //        if (trangThai == "Huy_Don" && !string.IsNullOrWhiteSpace(lyDoHuy))
        //            body.AppendLine($"Lý do hủy: {lyDoHuy}");
        //        body.AppendLine();
        //        body.AppendLine("Trân trọng,");
        //        body.AppendLine("Đội ngũ hỗ trợ");

        //        await emailService.SendEmailAsync(hd.KhachHang.Email, subject, body.ToString());
        //    }

        //    return true;
        //}





        //Đổi trạng thái gửi gmail ở quản lý đơn hàng nhưng gửi gmail cho khách vãng lai nữa 
        //đoạn code này trừ số lượng trong kho khi xác nhận đơn hàng , nhưng luông mới Chua_Xac_Nhan đã trừ kho rồi phải command lại 
        //        public async Task<bool> UpdateTrangThaiAsync(int id, string trangThai, string? lyDoHuy, EmailService emailService)
        //        {
        //            try
        //            {
        //                // Truy vấn hóa đơn với các navigation cần thiết
        //                var hd = await _context.Hoa_Don
        //                    .Include(h => h.KhachHang)
        //                    .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.SanPham)
        //                    .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.HoaDonChiTietToppings).ThenInclude(hdctt => hdctt.Topping)
        //                    .FirstOrDefaultAsync(x => x.ID_Hoa_Don == id);

        //                if (hd == null) return false;

        //                if (trangThai == "Da_Xac_Nhan")
        //                {
        //                    foreach (var hdct in hd.HoaDonChiTiets)
        //                    {
        //                        var sanPham = hdct.SanPham;
        //                        if (sanPham == null)
        //                            throw new Exception($"Sản phẩm với ID {hdct.ID_San_Pham} không tồn tại.");
        //                        if (sanPham.So_Luong < hdct.So_Luong)
        //                            throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ số lượng.");

        //                        sanPham.So_Luong -= hdct.So_Luong;

        //                        if (hdct.HoaDonChiTietToppings?.Any() == true)
        //                        {
        //                            foreach (var topping in hdct.HoaDonChiTietToppings)
        //                            {
        //                                var toppingEntity = topping.Topping;
        //                                if (toppingEntity == null)
        //                                    throw new Exception($"Topping với ID {topping.ID_Topping} không tồn tại.");
        //                                if (toppingEntity.So_Luong < topping.So_Luong)
        //                                    throw new Exception($"Topping {toppingEntity.Ten} không đủ số lượng.");

        //                                toppingEntity.So_Luong -= topping.So_Luong ?? 0;
        //                            }
        //                        }
        //                    }
        //                }

        //                // Cập nhật trạng thái và lý do hủy
        //                hd.Trang_Thai = trangThai;
        //                if (!string.IsNullOrWhiteSpace(lyDoHuy) && trangThai == "Huy_Don")
        //                    hd.LyDoHuyDon = lyDoHuy;

        //                await _context.SaveChangesAsync();

        //                // Trích xuất email và họ tên
        //                string? email = null;
        //                string? hoTen = null;
        //                if (hd.KhachHang != null && !string.IsNullOrWhiteSpace(hd.KhachHang.Email))
        //                {
        //                    email = hd.KhachHang.Email;
        //                    hoTen = hd.KhachHang.Ho_Ten;
        //                }
        //                else if (!string.IsNullOrWhiteSpace(hd.Ghi_Chu))
        //                {
        //                    try
        //                    {
        //                        // Regex cho "Họ và Tên: [value]" (lấy đến dấu phẩy hoặc cuối chuỗi)
        //                        var hoTenMatch = Regex.Match(hd.Ghi_Chu, @"Họ và Tên\s*:\s*(.+?)(?=,\s*|\s*$)", RegexOptions.IgnoreCase);
        //                        if (hoTenMatch.Success)
        //                        {
        //                            hoTen = hoTenMatch.Groups[1].Value.Trim();
        //                        }

        //                        // Regex cho "Email: [value]" (lấy email hợp lệ, hỗ trợ .com hoặc các domain khác)
        //                        var emailMatch = Regex.Match(hd.Ghi_Chu, @"Email\s*:\s*([\w\.-]+@[\w\.-]+\.[\w\.-]+)", RegexOptions.IgnoreCase);
        //                        if (emailMatch.Success)
        //                        {
        //                            var potentialEmail = emailMatch.Groups[1].Value.Trim();
        //                            if (Regex.IsMatch(potentialEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        //                            {
        //                                email = potentialEmail;
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        // Log lỗi nếu cần
        //                        // _logger.LogError("Lỗi phân tích Ghi_Chu: {Message}", ex.Message);
        //                    }
        //                }

        //                // Gửi email thông báo
        //                if (!string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        //                {
        //                    var subject = $"Cập nhật trạng thái hóa đơn #{hd.Ma_Hoa_Don}";
        //                    var productList = string.Join("", hd.HoaDonChiTiets.Select(hdct =>
        //                    {
        //                        var toppingList = hdct.HoaDonChiTietToppings?.Any() == true
        //                            ? string.Join("", hdct.HoaDonChiTietToppings.Select(t =>
        //                                $"<li>Topping: {System.Security.SecurityElement.Escape(t.Topping?.Ten)} (Số lượng: {t.So_Luong}, Giá: {t.Gia_Topping:N0} VNĐ)</li>"))
        //                            : "<li>Không có topping</li>";
        //                        return $"<li>{System.Security.SecurityElement.Escape(hdct.SanPham?.Ten_San_Pham)} (Số lượng: {hdct.So_Luong}, Giá: {hdct.Gia_San_Pham:N0} VNĐ)<ul>{toppingList}</ul></li>";
        //                    }));
        //                    var body = @"<!DOCTYPE html>
        //<html lang=""vi"">
        //<head>
        //    <meta charset=""UTF-8"">
        //    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        //    <style>
        //        body { font-family: 'Segoe UI', Arial, sans-serif; color: #333; background: #f5f5f5; margin: 0; padding: 20px; }
        //        .container { max-width: 600px; margin: 0 auto; background: #fff; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
        //        .header { background: #1e88e5; color: #fff; padding: 15px; text-align: center; }
        //        .header h1 { font-size: 22px; margin: 0; }
        //        .content { padding: 20px; line-height: 1.6; }
        //        .content p { margin: 10px 0; }
        //        .highlight { font-weight: bold; color: #d32f2f; }
        //        .button { display: inline-block; padding: 10px 20px; background: #1e88e5; color: #fff; text-decoration: none; border-radius: 5px; margin: 10px 0; font-weight: 500; }
        //        .button:hover { background: #1565c0; }
        //        .footer { padding: 10px; text-align: center; font-size: 12px; color: #666; border-top: 1px solid #eee; }
        //        .logo { max-width: 150px; margin: 10px auto; display: block; }
        //        ul { margin: 10px 0; padding-left: 20px; }
        //        li { margin-bottom: 5px; }
        //    </style>
        //</head>
        //<body>
        //    <div class=""container"">
        //        <div class=""header"">
        //            <img src=""https://localhost:7081/Logo/logo.png"" alt=""Company Logo"" class=""logo"">
        //            <h1>Cập Nhật Trạng Thái Hóa Đơn</h1>
        //        </div>
        //        <div class=""content"">
        //            <p>Kính gửi " + System.Security.SecurityElement.Escape(hoTen ?? "Quý khách") + @",</p>
        //            <p>Hóa đơn của bạn với mã <span class=""highlight"">#" + hd.Ma_Hoa_Don + @"</span> đã được cập nhật trạng thái thành: <strong>" + System.Security.SecurityElement.Escape(trangThai) + @"</strong>.</p>
        //            <p><strong>Chi tiết sản phẩm và topping:</strong></p>
        //            <ul>" + productList + @"</ul>
        //            " + (trangThai == "Huy_Don" && !string.IsNullOrWhiteSpace(lyDoHuy) ? $"<p><strong>Lý do hủy:</strong> {System.Security.SecurityElement.Escape(lyDoHuy)}</p>" : "") + @"

        //            <p>Liên hệ chúng tôi qua email blackmoondoge@gmail.com hoặc hotline 0834119666 nếu bạn cần hỗ trợ.</p>
        //        </div>
        //        <div class=""footer"">
        //            <p>Trân trọng,<br>Trà Sữa TheBoy <br><a href=""https://localhost:7081/"">yourdomain.com</a></p>
        //        </div>
        //    </div>
        //</body>
        //</html>";

        //                    await emailService.SendEmailAsync(email, subject, body);
        //                }

        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log lỗi nếu cần
        //                // _logger.LogError("Lỗi cập nhật trạng thái hóa đơn {HoaDonId}: {Message}", id, ex.Message);
        //                return false;
        //            }
        //        }






        public async Task<bool> UpdateTrangThaiAsync(int id, string trangThai, string? lyDoHuy, EmailService emailService)
        {
            try
            {
                // Truy vấn hóa đơn với các navigation cần thiết
                var hd = await _context.Hoa_Don
                    .Include(h => h.KhachHang)
                    .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.SanPham)
                    .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.HoaDonChiTietToppings).ThenInclude(hdctt => hdctt.Topping)
                    .FirstOrDefaultAsync(x => x.ID_Hoa_Don == id);

                if (hd == null) return false;

                // Kiểm tra số lượng khi ở trạng thái Da_Xac_Nhan, nhưng không trừ
                if (trangThai == "Da_Xac_Nhan")
                {
                    foreach (var hdct in hd.HoaDonChiTiets)
                    {
                        var sanPham = hdct.SanPham;
                        if (sanPham == null)
                            throw new Exception($"Sản phẩm với ID {hdct.ID_San_Pham} không tồn tại.");
                        if (sanPham.So_Luong < hdct.So_Luong)
                            throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ số lượng.");

                        if (hdct.HoaDonChiTietToppings?.Any() == true)
                        {
                            foreach (var topping in hdct.HoaDonChiTietToppings)
                            {
                                var toppingEntity = topping.Topping;
                                if (toppingEntity == null)
                                    throw new Exception($"Topping với ID {topping.ID_Topping} không tồn tại.");
                                if (toppingEntity.So_Luong < topping.So_Luong)
                                    throw new Exception($"Topping {toppingEntity.Ten} không đủ số lượng.");
                            }
                        }
                    }
                }

                // Cập nhật trạng thái và lý do hủy
                hd.Trang_Thai = trangThai;
                if (!string.IsNullOrWhiteSpace(lyDoHuy) && trangThai == "Huy_Don")
                    hd.LyDoHuyDon = lyDoHuy;

                await _context.SaveChangesAsync();

                // Trích xuất email và họ tên
                string? email = null;
                string? hoTen = null;
                if (hd.KhachHang != null && !string.IsNullOrWhiteSpace(hd.KhachHang.Email))
                {
                    email = hd.KhachHang.Email;
                    hoTen = hd.KhachHang.Ho_Ten;
                }
                else if (!string.IsNullOrWhiteSpace(hd.Ghi_Chu))
                {
                    try
                    {
                        var hoTenMatch = Regex.Match(hd.Ghi_Chu, @"Họ và Tên\s*:\s*(.+?)(?=,\s*|\s*$)", RegexOptions.IgnoreCase);
                        if (hoTenMatch.Success)
                        {
                            hoTen = hoTenMatch.Groups[1].Value.Trim();
                        }

                        var emailMatch = Regex.Match(hd.Ghi_Chu, @"Email\s*:\s*([\w\.-]+@[\w\.-]+\.[\w\.-]+)", RegexOptions.IgnoreCase);
                        if (emailMatch.Success)
                        {
                            var potentialEmail = emailMatch.Groups[1].Value.Trim();
                            if (Regex.IsMatch(potentialEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            {
                                email = potentialEmail;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Có thể thêm logging nếu cần
                    }
                }

                // Gửi email thông báo
                if (!string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    var subject = $"Cập nhật trạng thái hóa đơn #{hd.Ma_Hoa_Don}";
                    var productList = string.Join("", hd.HoaDonChiTiets.Select(hdct =>
                    {
                        var toppingList = hdct.HoaDonChiTietToppings?.Any() == true
                            ? string.Join("", hdct.HoaDonChiTietToppings.Select(t =>
                                $"<li>Topping: {System.Security.SecurityElement.Escape(t.Topping?.Ten)} (Số lượng: {t.So_Luong}, Giá: {t.Gia_Topping:N0} VNĐ)</li>"))
                            : "<li>Không có topping</li>";
                        return $"<li>{System.Security.SecurityElement.Escape(hdct.SanPham?.Ten_San_Pham)} (Số lượng: {hdct.So_Luong}, Giá: {hdct.Gia_San_Pham:N0} VNĐ)<ul>{toppingList}</ul></li>";
                    }));

                    var body = $@"<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; color: #333; background: #f5f5f5; margin: 0; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background: #fff; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
        .header {{ background: #1e88e5; color: #fff; padding: 15px; text-align: center; }}
        .header h1 {{ font-size: 22px; margin: 0; }}
        .content {{ padding: 20px; line-height: 1.6; }}
        .content p {{ margin: 10px 0; }}
        .highlight {{ font-weight: bold; color: #d32f2f; }}
        .button {{ display: inline-block; padding: 10px 20px; background: #1e88e5; color: #fff; text-decoration: none; border-radius: 5px; margin: 10px 0; font-weight: 500; }}
        .button:hover {{ background: #1565c0; }}
        .footer {{ padding: 10px; text-align: center; font-size: 12px; color: #666; border-top: 1px solid #eee; }}
        .logo {{ max-width: 150px; margin: 10px auto; display: block; }}
        ul {{ margin: 10px 0; padding-left: 20px; }}
        li {{ margin-bottom: 5px; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <img src=""https://localhost:7081/Logo/logo.png"" alt=""Company Logo"" class=""logo"">
            <h1>Cập Nhật Trạng Thái Hóa Đơn</h1>
        </div>
        <div class=""content"">
            <p>Kính gửi {System.Security.SecurityElement.Escape(hoTen ?? "Quý khách")},</p>
            <p>Hóa đơn của bạn với mã <span class=""highlight"">#{hd.Ma_Hoa_Don}</span> đã được cập nhật trạng thái thành: <strong>{System.Security.SecurityElement.Escape(trangThai)}</strong>.</p>
            <p><strong>Chi tiết sản phẩm và topping:</strong></p>
            <ul>{productList}</ul>
            {(trangThai == "Huy_Don" && !string.IsNullOrWhiteSpace(lyDoHuy) ? $"<p><strong>Lý do hủy:</strong> {System.Security.SecurityElement.Escape(lyDoHuy)}</p>" : "")}
            <p>Liên hệ chúng tôi qua email blackmoondoge@gmail.com hoặc hotline 0834119666 nếu bạn cần hỗ trợ.</p>
        </div>
        <div class=""footer"">
            <p>Trân trọng,<br>Trà Sữa TheBoy <br><a href=""https://localhost:7081/"">yourdomain.com</a></p>
        </div>
    </div>
</body>
</html>";

                    await emailService.SendEmailAsync(email, subject, body);
                }

                return true;
            }
            catch (Exception)
            {
                // Có thể thêm logging nếu cần
                return false;
            }
        }




        // ==== CancelWithRestockAsync: (như bản bạn đã đưa) ====
        //public async Task<bool> CancelWithRestockAsync(
        //    int hoaDonId,
        //    string lyDoHuy,
        //    IEnumerable<(int chiTietId, int quantity)> selections)
        //{
        //    using var tx = await _context.Database.BeginTransactionAsync();

        //    var hd = await _context.Hoa_Don
        //        .Include(h => h.KhachHang).ThenInclude(kh => kh.KhachHangVouchers)
        //        .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.HoaDonChiTietToppings).ThenInclude(ctt => ctt.Topping)
        //        .Include(h => h.HoaDonVouchers).ThenInclude(hdv => hdv.Voucher)
        //        .FirstOrDefaultAsync(h => h.ID_Hoa_Don == hoaDonId);

        //    if (hd == null) return false;
        //    if (hd.Trang_Thai == "Huy_Don" || hd.Trang_Thai == "Hoan_Thanh") return false;

        //    var ctMap = hd.HoaDonChiTiets.ToDictionary(x => x.ID_HoaDon_ChiTiet, x => x);

        //    if (hd.Trang_Thai == "Chua_Xac_Nhan")
        //    {
        //        if (hd.HoaDonVouchers != null && hd.HoaDonVouchers.Any())
        //        {
        //            var khachHangVoucher = hd.KhachHang?.KhachHangVouchers
        //                .FirstOrDefault(khv => khv.ID_Voucher == hd.HoaDonVouchers.First().ID_Voucher);
        //            if (khachHangVoucher != null)
        //            {
        //                khachHangVoucher.Trang_Thai = true;
        //                _context.KhachHang_Voucher.Update(khachHangVoucher);
        //            }
        //        }
        //    }
        //    else if (hd.Trang_Thai == "Da_Xac_Nhan")
        //    {
        //        foreach (var (chiTietId, quantity) in selections ?? Enumerable.Empty<(int, int)>())
        //        {
        //            if (!ctMap.TryGetValue(chiTietId, out var ct)) continue;
        //            var qty = Math.Clamp(quantity, 0, ct.So_Luong);
        //            if (qty <= 0) continue;

        //            var sp = await _context.San_Pham.FirstOrDefaultAsync(s => s.ID_San_Pham == ct.ID_San_Pham);
        //            if (sp != null)
        //            {
        //                sp.So_Luong += qty;
        //                _context.San_Pham.Update(sp);
        //            }

        //            foreach (var toppingDetail in ct.HoaDonChiTietToppings)
        //            {
        //                var topping = toppingDetail.Topping;
        //                if (topping != null && toppingDetail.So_Luong.HasValue)
        //                {
        //                    topping.So_Luong += toppingDetail.So_Luong.Value * qty;
        //                    _context.Topping.Update(topping);
        //                }
        //            }
        //        }

        //        if (hd.HoaDonVouchers != null && hd.HoaDonVouchers.Any())
        //        {
        //            var khachHangVoucher = hd.KhachHang?.KhachHangVouchers
        //                .FirstOrDefault(khv => khv.ID_Voucher == hd.HoaDonVouchers.First().ID_Voucher);
        //            if (khachHangVoucher != null)
        //            {
        //                khachHangVoucher.Trang_Thai = true;
        //                _context.KhachHang_Voucher.Update(khachHangVoucher);
        //            }
        //        }
        //    }

        //    hd.Trang_Thai = "Huy_Don";
        //    hd.LyDoHuyDon = string.IsNullOrWhiteSpace(lyDoHuy) ? "Không rõ lý do" : lyDoHuy;
        //    _context.Hoa_Don.Update(hd);

        //    await _context.SaveChangesAsync();

        //    if (hd.KhachHang != null && !string.IsNullOrWhiteSpace(hd.KhachHang.Email))
        //    {
        //        var linkTraCuu = $"https://yourdomain.com/hoa-don/tra-cuu/{hd.Ma_Hoa_Don}";
        //        var subject = $"Hủy đơn hàng #{hd.Ma_Hoa_Don}";
        //        var body = new StringBuilder();
        //        body.AppendLine($"Kính gửi {hd.KhachHang.Ho_Ten},");
        //        body.AppendLine();
        //        body.AppendLine($"Đơn hàng của bạn (Mã: {hd.Ma_Hoa_Don}) đã bị hủy.");
        //        body.AppendLine($"Lý do hủy: {hd.LyDoHuyDon}");
        //        body.AppendLine();
        //        body.AppendLine($"Bạn có thể tra cứu chi tiết tại: {linkTraCuu}");
        //        body.AppendLine();
        //        body.AppendLine("Nếu có thắc mắc, vui lòng liên hệ chúng tôi.");
        //        body.AppendLine("Trân trọng,");

        //        await _emailService.SendEmailAsync(hd.KhachHang.Email, subject, body.ToString());
        //    }

        //    await tx.CommitAsync();
        //    return true;
        //}
        //CancelWithRestockAsync THÊM CHỨC NĂNG GỬI GMAIL CHO KHÁCH VÃNG LAI VỀ TRẠNG THÁI ĐƠN HÀNG 



        public async Task<bool> CancelWithRestockAsync(
    int hoaDonId,
    string lyDoHuy,
    IEnumerable<(int chiTietId, int quantity)> selections)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // Truy vấn hóa đơn với các navigation cần thiết
                var hd = await _context.Hoa_Don
                    .Include(h => h.KhachHang).ThenInclude(kh => kh!.KhachHangVouchers)
                    .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.HoaDonChiTietToppings).ThenInclude(ctt => ctt.Topping)
                    .Include(h => h.HoaDonVouchers).ThenInclude(hdv => hdv.Voucher)
                    .FirstOrDefaultAsync(h => h.ID_Hoa_Don == hoaDonId);

                if (hd == null || hd.Trang_Thai is "Huy_Don" or "Hoan_Thanh")
                {
                    return false;
                }

                var ctMap = hd.HoaDonChiTiets.ToDictionary(x => x.ID_HoaDon_ChiTiet, x => x);

                // Xử lý voucher và hoàn kho
                if (hd.Trang_Thai is "Chua_Xac_Nhan" or "Da_Xac_Nhan")
                {
                    if (hd.HoaDonVouchers?.Any() == true)
                    {
                        var khachHangVoucher = hd.KhachHang?.KhachHangVouchers
                            .FirstOrDefault(khv => khv.ID_Voucher == hd.HoaDonVouchers.First().ID_Voucher);
                        if (khachHangVoucher != null)
                        {
                            khachHangVoucher.Trang_Thai = true;
                            _context.KhachHang_Voucher.Update(khachHangVoucher);
                        }
                    }

                    if (hd.Trang_Thai == "Da_Xac_Nhan")
                    {
                        foreach (var (chiTietId, quantity) in selections ?? Enumerable.Empty<(int, int)>())
                        {
                            if (!ctMap.TryGetValue(chiTietId, out var ct)) continue;
                            var qty = Math.Clamp(quantity, 0, ct.So_Luong);
                            if (qty <= 0) continue;

                            var sp = await _context.San_Pham.FirstOrDefaultAsync(s => s.ID_San_Pham == ct.ID_San_Pham);
                            if (sp != null)
                            {
                                sp.So_Luong += qty;
                                _context.San_Pham.Update(sp);
                            }

                            foreach (var toppingDetail in ct.HoaDonChiTietToppings)
                            {
                                var topping = toppingDetail.Topping;
                                if (topping != null && toppingDetail.So_Luong.HasValue)
                                {
                                    topping.So_Luong += toppingDetail.So_Luong.Value * qty;
                                    _context.Topping.Update(topping);
                                }
                            }
                        }
                    }
                }

                // Cập nhật trạng thái hóa đơn
                hd.Trang_Thai = "Huy_Don";
                hd.LyDoHuyDon = string.IsNullOrWhiteSpace(lyDoHuy) ? "Không rõ lý do" : lyDoHuy;
                _context.Hoa_Don.Update(hd);

                await _context.SaveChangesAsync();

                // Trích xuất email và họ tên
                string? email = null;
                string? hoTen = null;
                if (hd.KhachHang != null && !string.IsNullOrWhiteSpace(hd.KhachHang.Email))
                {
                    email = hd.KhachHang.Email;
                    hoTen = hd.KhachHang.Ho_Ten;
                }
                else if (!string.IsNullOrWhiteSpace(hd.Ghi_Chu))
                {
                    try
                    {
                        // Regex cho "Họ và Tên: [value]" (lấy đến dấu phẩy hoặc cuối chuỗi)
                        var hoTenMatch = Regex.Match(hd.Ghi_Chu, @"Họ và Tên\s*:\s*(.+?)(?=,\s*|\s*$)", RegexOptions.IgnoreCase);
                        if (hoTenMatch.Success)
                        {
                            hoTen = hoTenMatch.Groups[1].Value.Trim();
                        }

                        // Regex cho "Email: [value]" (lấy email hợp lệ bắt đầu sau Email: và kết thúc bằng .com hoặc domain khác, bỏ qua khoảng trắng thừa)
                        var emailMatch = Regex.Match(hd.Ghi_Chu, @"Email\s*:\s*([\w\.-]+@[\w\.-]+\.[\w\.-]+)", RegexOptions.IgnoreCase);
                        if (emailMatch.Success)
                        {
                            var potentialEmail = emailMatch.Groups[1].Value.Trim();
                            if (Regex.IsMatch(potentialEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            {
                                email = potentialEmail;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi nếu cần
                        // _logger.LogError("Lỗi phân tích Ghi_Chu: {Message}", ex.Message);
                    }
                }

                // Gửi email thông báo
                if (!string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    var subject = $"Hủy đơn hàng #{hd.Ma_Hoa_Don}";
                    var body = @"<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; color: #333; background: #f5f5f5; margin: 0; padding: 20px; }
        .container { max-width: 600px; margin: 0 auto; background: #fff; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
        .header { background: #1e88e5; color: #fff; padding: 15px; text-align: center; }
        .header h1 { font-size: 22px; margin: 0; }
        .content { padding: 20px; line-height: 1.6; }
        .content p { margin: 10px 0; }
        .highlight { font-weight: bold; color: #d32f2f; }
        .button { display: inline-block; padding: 10px 20px; background: #1e88e5; color: #fff; text-decoration: none; border-radius: 5px; margin: 10px 0; font-weight: 500; }
        .button:hover { background: #1565c0; }
        .footer { padding: 10px; text-align: center; font-size: 12px; color: #666; border-top: 1px solid #eee; }
        .logo { max-width: 150px; margin: 10px auto; display: block; }
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <img src=""https://yourdomain.com/logo.png"" alt=""Company Logo"" class=""logo"">
            <h1>Thông Báo Hủy Đơn Hàng</h1>
        </div>
        <div class=""content"">
            <p>Kính gửi " + System.Security.SecurityElement.Escape(hoTen ?? "Quý khách") + @",</p>
            <p>Đơn hàng của bạn với mã <span class=""highlight"">#" + hd.Ma_Hoa_Don + @"</span> đã được hủy.</p>
            <p><strong>Lý do:</strong> " + System.Security.SecurityElement.Escape(hd.LyDoHuyDon) + @"</p>
            <p>Liên hệ chúng tôi qua email support@yourdomain.com hoặc hotline 0123-456-789 nếu bạn cần hỗ trợ.</p>
        </div>
        <div class=""footer"">
        </div>
    </div>
</body>
</html>";

                    await _emailService.SendEmailAsync(email, subject, body);
                }

                await tx.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                // Log lỗi nếu cần
                return false;
            }
        }

        // ==== VNPAY ====
        public async Task<HoaDon?> GetByMaHoaDonAsync(string maHoaDon)
            => await _context.Hoa_Don.FirstOrDefaultAsync(x => x.Ma_Hoa_Don == maHoaDon);

        //Hiện để phòng việc 2 người thanh toán vnpay 1 lúc nên khi trạng thái là CHƯA THANH TOÁN là đã trừ rồi,
        // nếu thanh toán thành công thì chỉ cần đổi trạng thái thôi; còn nếu thất bại/hủy thì hoàn trả.
        public async Task UpdateAsync(HoaDon hoaDon, string? vnPayResponseCode = null)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            // Load đầy đủ dữ liệu liên quan
            var hd = await _context.Hoa_Don
                .Include(h => h.KhachHang).ThenInclude(kh => kh.KhachHangVouchers)
                .Include(h => h.HoaDonVouchers).ThenInclude(hdv => hdv.Voucher)
                .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.SanPham)
                .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.HoaDonChiTietToppings).ThenInclude(hdctt => hdctt.Topping)
                .FirstOrDefaultAsync(h => h.ID_Hoa_Don == hoaDon.ID_Hoa_Don);

            if (hd == null) return;

            if (!string.IsNullOrEmpty(vnPayResponseCode) && vnPayResponseCode == "00")
            {
                // ✅ Giao dịch thành công: đã trừ kho từ trước, chỉ đổi trạng thái
                hd.Trang_Thai = "Da_Xac_Nhan";
            }
            else
            {
                // ❌ Thất bại/hủy: hoàn trả tồn + voucher
                hd.Trang_Thai = "Huy_Don";
                hd.LyDoHuyDon = "Hủy Thanh Toán VNPAY";

                foreach (var hdct in hd.HoaDonChiTiets)
                {
                    if (hdct.SanPham != null)
                    {
                        hdct.SanPham.So_Luong += hdct.So_Luong;
                        _context.San_Pham.Update(hdct.SanPham);
                    }

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

                if (hd.HoaDonVouchers != null && hd.HoaDonVouchers.Any())
                {
                    var khachHangVoucher = hd.KhachHang?.KhachHangVouchers
                        .FirstOrDefault(khv => khv.ID_Voucher == hd.HoaDonVouchers.First().ID_Voucher);

                    if (khachHangVoucher != null)
                    {
                        khachHangVoucher.Trang_Thai = true;
                        _context.KhachHang_Voucher.Update(khachHangVoucher);
                    }
                }
            }

            _context.Hoa_Don.Update(hd);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }
    }
}
