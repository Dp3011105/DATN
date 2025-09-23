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

        //public async Task<KhuyenMai> UpdateAsync(int id, KhuyenMaiDTO khuyenMaiDTO)
        //{
        //    var existing = await _context.KhuyenMai.FirstOrDefaultAsync(km => km.ID_Khuyen_Mai == id);
        //    if (existing == null)
        //    {
        //        return null;
        //    }

        //    existing.Ten_Khuyen_Mai = khuyenMaiDTO.Ten_Khuyen_Mai;
        //    existing.Ngay_Bat_Dau = khuyenMaiDTO.Ngay_Bat_Dau;
        //    existing.Ngay_Ket_Thuc = khuyenMaiDTO.Ngay_Ket_Thuc;
        //    existing.Mo_Ta = khuyenMaiDTO.Mo_Ta;
        //    existing.Trang_Thai = khuyenMaiDTO.Trang_Thai;

        //    await _context.SaveChangesAsync();
        //    return existing;
        //}


        // vì sai phân tích ERD vậy nên phải thực hiện xóa thẳng vào báng SanPhamKhuyenMai khi đổi trạng thái từ true sang false


        public async Task<KhuyenMai> UpdateAsync(int id, KhuyenMaiDTO khuyenMaiDTO)
        {
            var existing = await _context.KhuyenMai
                .Include(km => km.SanPhamKhuyenMais)
                .FirstOrDefaultAsync(km => km.ID_Khuyen_Mai == id);

            if (existing == null)
            {
                return null;
            }

            // Cập nhật các thuộc tính
            existing.Ten_Khuyen_Mai = khuyenMaiDTO.Ten_Khuyen_Mai;
            existing.Ngay_Bat_Dau = khuyenMaiDTO.Ngay_Bat_Dau;
            existing.Ngay_Ket_Thuc = khuyenMaiDTO.Ngay_Ket_Thuc;
            existing.Mo_Ta = khuyenMaiDTO.Mo_Ta;

            // Kiểm tra trạng thái thay đổi từ true sang false
            if (existing.Trang_Thai && !khuyenMaiDTO.Trang_Thai)
            {
                // Xóa các bản ghi trong SanPhamKhuyenMai có ID_Khuyen_Mai = id
                var recordsToDelete = existing.SanPhamKhuyenMais
                    .Where(spkm => spkm.ID_Khuyen_Mai == id)
                    .ToList();

                _context.SanPhamKhuyenMai.RemoveRange(recordsToDelete);
            }

            existing.Trang_Thai = khuyenMaiDTO.Trang_Thai;

            await _context.SaveChangesAsync();
            return existing;
        }









    }
}
