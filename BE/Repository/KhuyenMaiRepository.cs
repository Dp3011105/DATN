using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class KhuyenMaiRepository : IKhuyenMaiRepository
    {
        private readonly MyDbContext _context;

        public KhuyenMaiRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<KhuyenMaiDTO>> GetAllAsync()
        {
            return await _context.KhuyenMai
                .Select(km => new KhuyenMaiDTO
                {
                    ID_Khuyen_Mai = km.ID_Khuyen_Mai,
                    Ten_Khuyen_Mai = km.Ten_Khuyen_Mai,
                    Ngay_Bat_Dau = km.Ngay_Bat_Dau,
                    Ngay_Ket_Thuc = km.Ngay_Ket_Thuc,
                    Mo_Ta = km.Mo_Ta,
                    Trang_Thai = km.Trang_Thai
                })
                .ToListAsync();
        }

        public async Task<KhuyenMaiDTO> GetByIdAsync(int id)
        {
            return await _context.KhuyenMai
                .Where(km => km.ID_Khuyen_Mai == id)
                .Select(km => new KhuyenMaiDTO
                {
                    ID_Khuyen_Mai = km.ID_Khuyen_Mai,
                    Ten_Khuyen_Mai = km.Ten_Khuyen_Mai,
                    Ngay_Bat_Dau = km.Ngay_Bat_Dau,
                    Ngay_Ket_Thuc = km.Ngay_Ket_Thuc,
                    Mo_Ta = km.Mo_Ta,
                    Trang_Thai = km.Trang_Thai
                })
                .FirstOrDefaultAsync();
        }

        public async Task<KhuyenMai> CreateAsync(KhuyenMaiDTO khuyenMaiDTO)
        {
            var khuyenMai = new KhuyenMai
            {
                Ten_Khuyen_Mai = khuyenMaiDTO.Ten_Khuyen_Mai,
                Ngay_Bat_Dau = khuyenMaiDTO.Ngay_Bat_Dau,
                Ngay_Ket_Thuc = khuyenMaiDTO.Ngay_Ket_Thuc,
                Mo_Ta = khuyenMaiDTO.Mo_Ta,
                Trang_Thai = khuyenMaiDTO.Trang_Thai
            };

            _context.KhuyenMai.Add(khuyenMai);
            await _context.SaveChangesAsync();
            return khuyenMai;
        }

        public async Task<KhuyenMai> UpdateAsync(int id, KhuyenMaiDTO khuyenMaiDTO)
        {
            var existing = await _context.KhuyenMai.FirstOrDefaultAsync(km => km.ID_Khuyen_Mai == id);
            if (existing == null)
            {
                return null;
            }

            existing.Ten_Khuyen_Mai = khuyenMaiDTO.Ten_Khuyen_Mai;
            existing.Ngay_Bat_Dau = khuyenMaiDTO.Ngay_Bat_Dau;
            existing.Ngay_Ket_Thuc = khuyenMaiDTO.Ngay_Ket_Thuc;
            existing.Mo_Ta = khuyenMaiDTO.Mo_Ta;
            existing.Trang_Thai = khuyenMaiDTO.Trang_Thai;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
