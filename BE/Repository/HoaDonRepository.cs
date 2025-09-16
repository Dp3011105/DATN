﻿using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .Include(hd => hd.DiaChi) // bổ sung nếu cần hiển thị chi tiết địa chỉ
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
        public async Task<bool> UpdateTrangThaiAsync(int id, string trangThai, string? lyDoHuy, EmailService emailService)
        {
            var hd = await _context.Hoa_Don
                .Include(h => h.KhachHang)
                .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.SanPham)
                .Include(h => h.HoaDonChiTiets).ThenInclude(hdct => hdct.HoaDonChiTietToppings).ThenInclude(hdctt => hdctt.Topping)
                .FirstOrDefaultAsync(x => x.ID_Hoa_Don == id);

            if (hd == null) return false;

            if (trangThai == "Da_Xac_Nhan")
            {
                foreach (var hdct in hd.HoaDonChiTiets)
                {
                    var sanPham = hdct.SanPham;
                    if (sanPham == null)
                        throw new Exception($"Sản phẩm với ID {hdct.ID_San_Pham} không tồn tại.");
                    if (sanPham.So_Luong < hdct.So_Luong)
                        throw new Exception($"Sản phẩm {sanPham.Ten_San_Pham} không đủ số lượng.");

                    sanPham.So_Luong -= hdct.So_Luong;

                    if (hdct.HoaDonChiTietToppings != null && hdct.HoaDonChiTietToppings.Any())
                    {
                        foreach (var topping in hdct.HoaDonChiTietToppings)
                        {
                            var toppingEntity = topping.Topping;
                            if (toppingEntity == null)
                                throw new Exception($"Topping với ID {topping.ID_Topping} không tồn tại.");
                            if (toppingEntity.So_Luong < topping.So_Luong)
                                throw new Exception($"Topping {toppingEntity.Ten} không đủ số lượng.");

                            toppingEntity.So_Luong -= topping.So_Luong ?? 0;
                        }
                    }
                }
            }

            hd.Trang_Thai = trangThai;
            if (!string.IsNullOrWhiteSpace(lyDoHuy) && trangThai == "Huy_Don")
                hd.LyDoHuyDon = lyDoHuy;

            await _context.SaveChangesAsync();

            if (hd.KhachHang != null && !string.IsNullOrWhiteSpace(hd.KhachHang.Email))
            {
                var subject = $"Cập nhật trạng thái hóa đơn {hd.Ma_Hoa_Don}";
                var productList = string.Join("\n", hd.HoaDonChiTiets.Select(hdct =>
                {
                    var toppingList = hdct.HoaDonChiTietToppings != null && hdct.HoaDonChiTietToppings.Any()
                        ? string.Join("\n", hdct.HoaDonChiTietToppings.Select(t =>
                            $"  - Topping: {t.Topping?.Ten} (Số lượng: {t.So_Luong}, Giá: {t.Gia_Topping})"))
                        : "  - Không có topping";
                    return $"- {hdct.SanPham?.Ten_San_Pham} (Số lượng: {hdct.So_Luong}, Giá: {hdct.Gia_San_Pham})\n{toppingList}";
                }));
                var body = new StringBuilder();
                body.AppendLine($"Kính gửi {hd.KhachHang.Ho_Ten},");
                body.AppendLine();
                body.AppendLine($"Hóa đơn {hd.Ma_Hoa_Don} của bạn đã được cập nhật trạng thái thành: {trangThai}.");
                body.AppendLine("Chi tiết sản phẩm và topping:");
                body.AppendLine(productList);
                if (trangThai == "Huy_Don" && !string.IsNullOrWhiteSpace(lyDoHuy))
                    body.AppendLine($"Lý do hủy: {lyDoHuy}");
                body.AppendLine();
                body.AppendLine("Trân trọng,");
                body.AppendLine("Đội ngũ hỗ trợ");

                await emailService.SendEmailAsync(hd.KhachHang.Email, subject, body.ToString());
            }

            return true;
        }

        // ==== CancelWithRestockAsync: (như bản bạn đã đưa) ====
        public async Task<bool> CancelWithRestockAsync(
            int hoaDonId,
            string lyDoHuy,
            IEnumerable<(int chiTietId, int quantity)> selections)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var hd = await _context.Hoa_Don
                .Include(h => h.KhachHang).ThenInclude(kh => kh.KhachHangVouchers)
                .Include(h => h.HoaDonChiTiets).ThenInclude(ct => ct.HoaDonChiTietToppings).ThenInclude(ctt => ctt.Topping)
                .Include(h => h.HoaDonVouchers).ThenInclude(hdv => hdv.Voucher)
                .FirstOrDefaultAsync(h => h.ID_Hoa_Don == hoaDonId);

            if (hd == null) return false;
            if (hd.Trang_Thai == "Huy_Don" || hd.Trang_Thai == "Hoan_Thanh") return false;

            var ctMap = hd.HoaDonChiTiets.ToDictionary(x => x.ID_HoaDon_ChiTiet, x => x);

            if (hd.Trang_Thai == "Chua_Xac_Nhan")
            {
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
            else if (hd.Trang_Thai == "Da_Xac_Nhan")
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

            hd.Trang_Thai = "Huy_Don";
            hd.LyDoHuyDon = string.IsNullOrWhiteSpace(lyDoHuy) ? "Không rõ lý do" : lyDoHuy;
            _context.Hoa_Don.Update(hd);

            await _context.SaveChangesAsync();

            if (hd.KhachHang != null && !string.IsNullOrWhiteSpace(hd.KhachHang.Email))
            {
                var linkTraCuu = $"https://yourdomain.com/hoa-don/tra-cuu/{hd.Ma_Hoa_Don}";
                var subject = $"Hủy đơn hàng #{hd.Ma_Hoa_Don}";
                var body = new StringBuilder();
                body.AppendLine($"Kính gửi {hd.KhachHang.Ho_Ten},");
                body.AppendLine();
                body.AppendLine($"Đơn hàng của bạn (Mã: {hd.Ma_Hoa_Don}) đã bị hủy.");
                body.AppendLine($"Lý do hủy: {hd.LyDoHuyDon}");
                body.AppendLine();
                body.AppendLine($"Bạn có thể tra cứu chi tiết tại: {linkTraCuu}");
                body.AppendLine();
                body.AppendLine("Nếu có thắc mắc, vui lòng liên hệ chúng tôi.");
                body.AppendLine("Trân trọng,");

                await _emailService.SendEmailAsync(hd.KhachHang.Email, subject, body.ToString());
            }

            await tx.CommitAsync();
            return true;
        }

        // ==== VNPAY ====
        public async Task<HoaDon?> GetByMaHoaDonAsync(string maHoaDon)
            => await _context.Hoa_Don.FirstOrDefaultAsync(x => x.Ma_Hoa_Don == maHoaDon);

        public async Task UpdateAsync(HoaDon hoaDon, string? vnPayResponseCode = null)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

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
