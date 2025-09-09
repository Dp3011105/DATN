using BE.Data;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly MyDbContext _context;

        public VoucherRepository(MyDbContext context)
        {
            _context = context;
        }

        // SỬA: Sắp xếp theo thứ tự ưu tiên (Hoạt động -> Ngừng -> Hết hạn)
        public async Task<IEnumerable<Voucher>> GetAllAsync()
        {
            return await _context.Voucher
                .OrderBy(x => x.Ngay_Ket_Thuc.HasValue && x.Ngay_Ket_Thuc.Value < DateTime.Now ? 3 : (x.Trang_Thai == true ? 1 : 2))
                .ThenByDescending(x => x.ID_Voucher)
                .ToListAsync();
        }

        public async Task<Voucher?> GetByIdAsync(int id)
        {
            return await _context.Voucher.FindAsync(id);
        }

        public async Task<Voucher?> GetByCodeAsync(string code)
        {
            return await _context.Voucher
                .FirstOrDefaultAsync(x => x.Ma_Voucher == code);
        }

        public async Task<Voucher> CreateAsync(Voucher voucher)
        {
            _context.Voucher.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<Voucher> UpdateAsync(Voucher voucher)
        {
            _context.Entry(voucher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return voucher;
        }

        // THÊM: Method kích hoạt voucher
        public async Task<bool> ActivateAsync(int id)
        {
            var voucher = await GetByIdAsync(id);
            if (voucher == null) return false;

            voucher.Trang_Thai = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var voucher = await GetByIdAsync(id);
            if (voucher == null) return false;

            voucher.Trang_Thai = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Voucher.AnyAsync(x => x.ID_Voucher == id);
        }

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _context.Voucher.Where(x => x.Ma_Voucher == code);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.ID_Voucher != excludeId);
            }

            return await query.AnyAsync();
        }

        // THÊM: Lấy vouchers còn hiệu lực
        public async Task<IEnumerable<Voucher>> GetActiveVouchersAsync()
        {
            return await _context.Voucher
                .Where(x => x.Trang_Thai == true &&
                           (x.Ngay_Ket_Thuc == null || x.Ngay_Ket_Thuc > DateTime.Now))
                .OrderBy(x => x.Gia_Tri_Giam)
                .ToListAsync();
        }

        // THÊM: Kiểm tra voucher có thể sử dụng
        public async Task<bool> CanUseVoucherAsync(string code, decimal orderAmount)
        {
            var voucher = await GetByCodeAsync(code);

            if (voucher == null || voucher.Trang_Thai != true)
                return false;

            // Kiểm tra hết hạn
            if (voucher.Ngay_Ket_Thuc.HasValue && voucher.Ngay_Ket_Thuc.Value < DateTime.Now)
                return false;

            // Kiểm tra yêu cầu tối thiểu
            if (voucher.So_Tien_Dat_Yeu_Cau.HasValue && orderAmount < voucher.So_Tien_Dat_Yeu_Cau.Value)
                return false;

            // Kiểm tra số lượng
            if (voucher.So_Luong.HasValue && voucher.So_Luong.Value <= 0)
                return false;

            return true;
        }
    }
}