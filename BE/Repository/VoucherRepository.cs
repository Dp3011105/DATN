using BE.Data;
using BE.DTOs;
using BE.models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly MyDbContext _context;

        public VoucherRepository(MyDbContext context)
        {
            _context = context;
        }
       

        public async Task<IEnumerable<VoucherDTO>> GetAllAsync()
        {
            return await _context.Voucher
                .Select(v => new VoucherDTO
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

        public async Task<VoucherDTO> GetByIdAsync(int id)
        {
            var voucher = await _context.Voucher
                .Where(v => v.ID_Voucher == id)
                .Select(v => new VoucherDTO
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
                .FirstOrDefaultAsync();

            if (voucher == null)
            {
                throw new KeyNotFoundException($"Voucher with ID {id} not found.");
            }

            return voucher;
        }

        public async Task<VoucherDTO> AddAsync(VoucherDTO voucherDTO)
        {
            if (await _context.Voucher.AnyAsync(v => v.Ma_Voucher == voucherDTO.Ma_Voucher))
            {
                throw new InvalidOperationException("Mã voucher đã tồn tại.");
            }

            var voucher = new Voucher
            {
                Ma_Voucher = voucherDTO.Ma_Voucher,
                Ten = voucherDTO.Ten,
                So_Luong = voucherDTO.So_Luong,
                Gia_Tri_Giam = voucherDTO.Gia_Tri_Giam,
                So_Tien_Dat_Yeu_Cau = voucherDTO.So_Tien_Dat_Yeu_Cau,
                Ngay_Bat_Dau = voucherDTO.Ngay_Bat_Dau,
                Ngay_Ket_Thuc = voucherDTO.Ngay_Ket_Thuc,
                Trang_Thai = voucherDTO.Trang_Thai ?? true
            };

            _context.Voucher.Add(voucher);
            await _context.SaveChangesAsync();

            voucherDTO.ID_Voucher = voucher.ID_Voucher;
            return voucherDTO;
        }

        public async Task<VoucherDTO> UpdateAsync(int id, VoucherDTO voucherDTO)
        {
            var voucher = await _context.Voucher.FindAsync(id);
            if (voucher == null)
            {
                throw new KeyNotFoundException($"Voucher with ID {id} not found.");
            }

            if (await _context.Voucher.AnyAsync(v => v.Ma_Voucher == voucherDTO.Ma_Voucher && v.ID_Voucher != id))
            {
                throw new InvalidOperationException("Mã voucher đã tồn tại.");
            }

            voucher.Ma_Voucher = voucherDTO.Ma_Voucher;
            voucher.Ten = voucherDTO.Ten;
            voucher.So_Luong = voucherDTO.So_Luong;
            voucher.Gia_Tri_Giam = voucherDTO.Gia_Tri_Giam;
            voucher.So_Tien_Dat_Yeu_Cau = voucherDTO.So_Tien_Dat_Yeu_Cau;
            voucher.Ngay_Bat_Dau = voucherDTO.Ngay_Bat_Dau;
            voucher.Ngay_Ket_Thuc = voucherDTO.Ngay_Ket_Thuc;
            voucher.Trang_Thai = voucherDTO.Trang_Thai ?? true;

            await _context.SaveChangesAsync();
            return voucherDTO;
        }
    }
}