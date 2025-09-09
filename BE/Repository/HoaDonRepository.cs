namespace BE.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using BE.Data;
    using BE.DTOs;
    using BE.models;
    using BE.Repository.IRepository;

    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly MyDbContext _context;
        public HoaDonRepository(MyDbContext context) => _context = context;

        // ===== DANH SÁCH DẠNG DTO (cho UI list) =====
        public async Task<IEnumerable<HoaDonDTO>> GetAllAsync()
        {
            return await _context.Hoa_Don
                .Include(h => h.HinhThucThanhToan) // nếu có navigation
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
                                     .OrderByDescending(x => x.Ngay_Tao)
                                     .ToListAsync();

        // ===== CHI TIẾT =====
        public async Task<HoaDon> GetByIdAsync(int id)
        {
            return await _context.Hoa_Don
                .AsNoTracking()
                .Include(hd => hd.KhachHang)
                .Include(hd => hd.HinhThucThanhToan)
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

        public async Task<bool> UpdateTrangThaiAsync(int id, string trangThai, string? lyDoHuy)
        {
            var hd = await _context.Hoa_Don.FirstOrDefaultAsync(x => x.ID_Hoa_Don == id);
            if (hd == null) return false;

            hd.Trang_Thai = trangThai;
            if (!string.IsNullOrWhiteSpace(lyDoHuy) && trangThai == "Huy_Don")
                hd.LyDoHuyDon = lyDoHuy;

            await _context.SaveChangesAsync();
            return true;
        }

        // ===== NEW: HỦY + HOÀN TRẢ TỒN KHO =====
        public async Task<bool> CancelWithRestockAsync(
            int hoaDonId,
            string lyDoHuy,
            IEnumerable<(int chiTietId, int quantity)> selections
        )
        {
            // Transaction để đảm bảo tính toàn vẹn
            using var tx = await _context.Database.BeginTransactionAsync();

            // Tải đơn + chi tiết
            var hd = await _context.Hoa_Don
                .Include(h => h.HoaDonChiTiets)
                .FirstOrDefaultAsync(h => h.ID_Hoa_Don == hoaDonId);

            if (hd == null) return false;

            // Không cho hủy nếu đã Hủy/Hoàn thành
            if (hd.Trang_Thai == "Huy_Don" || hd.Trang_Thai == "Hoan_Thanh")
                return false;

            // Map nhanh chi tiết
            var ctMap = hd.HoaDonChiTiets.ToDictionary(x => x.ID_HoaDon_ChiTiet, x => x);

            // Cộng trả tồn kho theo selections
            foreach (var (chiTietId, quantity) in selections ?? Enumerable.Empty<(int, int)>())
            {
                if (!ctMap.TryGetValue(chiTietId, out var ct)) continue;

                var qty = quantity;
                if (qty < 0) qty = 0;
                if (qty > ct.So_Luong) qty = ct.So_Luong; // chặn vượt quá đã đặt
                if (qty <= 0) continue;

                // + tồn kho sản phẩm (giả định: SanPham có So_Luong_Ton, HoaDonChiTiet có ID_San_Pham)
                // Nếu bạn chỉ có navigation SanPham, vẫn cập nhật được thông qua ID:
                var sp = await _context.San_Pham.FirstOrDefaultAsync(s => s.ID_San_Pham == ct.ID_San_Pham);
                if (sp != null)
                {
                    sp.So_Luong = (sp.So_Luong ) + qty;
                    _context.San_Pham.Update(sp);
                }

                // Nếu bạn quản lý tồn kho theo topping/nguyên liệu, cộng trả ở đây (tuỳ schema):
                // var toppings = await _context.HoaDonChiTietTopping
                //     .Where(t => t.ID_HoaDon_ChiTiet == ct.ID_HoaDon_ChiTiet).ToListAsync();
                // foreach (var tpLine in toppings)
                // {
                //     var tp = await _context.Topping.FirstOrDefaultAsync(t => t.ID_Topping == tpLine.ID_Topping);
                //     if (tp != null)
                //     {
                //         tp.So_Luong_Ton = (tp.So_Luong_Ton ?? 0) + qty; // hoặc nhân theo định mức
                //         _context.Topping.Update(tp);
                //     }
                // }
            }

            // Ghi lý do hủy & đổi trạng thái
            hd.Trang_Thai = "Huy_Don";
            if (!string.IsNullOrWhiteSpace(lyDoHuy))
                hd.LyDoHuyDon = lyDoHuy;

            _context.Hoa_Don.Update(hd);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }
    }
}
