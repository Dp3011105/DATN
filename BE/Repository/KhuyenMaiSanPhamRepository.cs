using BE.Data;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class KhuyenMaiSanPhamRepository : IKhuyenMaiSanPhamRepository
    {
        private readonly MyDbContext _context;

        public KhuyenMaiSanPhamRepository(MyDbContext context)
        {
            _context = context;
        }
        // Gán khuyến mãi cho sản phẩm  
        public async Task AssignKhuyenMaiToProductsAsync(int idKhuyenMai, List<int> idSanPhams, decimal phanTramGiam)
        {
            var khuyenMai = await _context.KhuyenMai.FindAsync(idKhuyenMai);
            if (khuyenMai == null)
            {
                throw new Exception("Khuyến mãi không tồn tại");
            }

            foreach (var idSanPham in idSanPhams)
            {
                var sanPham = await _context.San_Pham.FindAsync(idSanPham);
                if (sanPham == null)
                {
                    continue; // Bỏ qua nếu sản phẩm không tồn tại
                }

                var existing = await _context.SanPhamKhuyenMai
                    .FirstOrDefaultAsync(spkm => spkm.ID_San_Pham == idSanPham && spkm.ID_Khuyen_Mai == idKhuyenMai);

                if (existing != null)
                {
                    continue; // Đã tồn tại, bỏ qua
                }

                var giaGiam = sanPham.Gia * (1 - phanTramGiam / 100);

                var spkm = new SanPhamKhuyenMai
                {
                    ID_San_Pham = idSanPham,
                    ID_Khuyen_Mai = idKhuyenMai,
                    Phan_Tram_Giam = phanTramGiam,
                    Gia_Giam = giaGiam
                };

                _context.SanPhamKhuyenMai.Add(spkm);
            }

            await _context.SaveChangesAsync();
        }
        // Xóa khuyến mãi khỏi sản phẩm
        public async Task RemoveKhuyenMaiFromProductsAsync(int idKhuyenMai, List<int> idSanPhams)
        {
            var khuyenMai = await _context.KhuyenMai.FindAsync(idKhuyenMai);
            if (khuyenMai == null)
            {
                throw new Exception("Khuyến mãi không tồn tại");
            }

            foreach (var idSanPham in idSanPhams)
            {
                var spkm = await _context.SanPhamKhuyenMai
                    .FirstOrDefaultAsync(spkm => spkm.ID_San_Pham == idSanPham && spkm.ID_Khuyen_Mai == idKhuyenMai);

                if (spkm != null)
                {
                    _context.SanPhamKhuyenMai.Remove(spkm);
                }
            }

            await _context.SaveChangesAsync();
        }


        // Lấy tất cả sản phẩm có khuyến mãi theo ID khuyến mãi
        public async Task<IEnumerable<object>> GetSanPhamByKhuyenMai(int idKhuyenMai)
        {
            return await _context.San_Pham
                .Where(sp => sp.Trang_Thai == true)
                .Join(_context.SanPhamKhuyenMai,
                    sp => sp.ID_San_Pham,
                    spkm => spkm.ID_San_Pham,
                    (sp, spkm) => new { SanPham = sp, SanPhamKhuyenMai = spkm })
                .Where(x => x.SanPhamKhuyenMai.ID_Khuyen_Mai == idKhuyenMai)
                .Select(x => new
                {
                    x.SanPham.ID_San_Pham,
                    x.SanPham.Ten_San_Pham,
                    x.SanPham.Gia,
                    x.SanPham.So_Luong,
                    x.SanPham.Hinh_Anh,
                    x.SanPham.Trang_Thai,
                    SanPhamKhuyenMai = new
                    {
                        x.SanPhamKhuyenMai.ID_San_Pham_Khuyen_Mai,
                        x.SanPhamKhuyenMai.Phan_Tram_Giam,
                        x.SanPhamKhuyenMai.Gia_Giam,
                        KhuyenMai = new
                        {
                            x.SanPhamKhuyenMai.BangKhuyenMai.ID_Khuyen_Mai,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Ten_Khuyen_Mai,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Ngay_Bat_Dau,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Ngay_Ket_Thuc,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Mo_Ta,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Trang_Thai
                        }
                    }
                })
                .ToListAsync();
        }

        // Hủy khuyến mãi cho sản phẩm cụ thể
        public async Task<bool> HuyKhuyenMai(int idSanPham, int idKhuyenMai)
        {
            var sanPhamKhuyenMai = await _context.SanPhamKhuyenMai
                .FirstOrDefaultAsync(spkm => spkm.ID_San_Pham == idSanPham && spkm.ID_Khuyen_Mai == idKhuyenMai);

            if (sanPhamKhuyenMai == null)
            {
                return false; // Không tìm thấy mối quan hệ giữa ID_San_Pham và ID_Khuyen_Mai
            }

            _context.SanPhamKhuyenMai.Remove(sanPhamKhuyenMai);
            await _context.SaveChangesAsync();
            return true;
        }




        // Lấy tất cả sản phẩm có khuyến mãi, bao gồm thông tin khuyến mãi nếu có
        public async Task<IEnumerable<object>> GetAllSanPhamWithKhuyenMai()
        {
            return await _context.San_Pham
                .Where(sp => sp.Trang_Thai == true)
                .GroupJoin(_context.SanPhamKhuyenMai,
                    sp => sp.ID_San_Pham,
                    spkm => spkm.ID_San_Pham,
                    (sp, spkm) => new { SanPham = sp, SanPhamKhuyenMais = spkm })
                .SelectMany(
                    x => x.SanPhamKhuyenMais.DefaultIfEmpty(),
                    (sp, spkm) => new
                    {
                        SanPham = sp.SanPham,
                        SanPhamKhuyenMai = spkm
                    })
                .Select(x => new
                {
                    x.SanPham.ID_San_Pham,
                    x.SanPham.Ten_San_Pham,
                    x.SanPham.Gia,
                    x.SanPham.So_Luong,
                    x.SanPham.Hinh_Anh,
                    x.SanPham.Trang_Thai,
                    SanPhamKhuyenMai = x.SanPhamKhuyenMai != null ? new
                    {
                        x.SanPhamKhuyenMai.ID_San_Pham_Khuyen_Mai,
                        x.SanPhamKhuyenMai.Phan_Tram_Giam,
                        x.SanPhamKhuyenMai.Gia_Giam,
                        KhuyenMai = new
                        {
                            x.SanPhamKhuyenMai.BangKhuyenMai.ID_Khuyen_Mai,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Ten_Khuyen_Mai,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Ngay_Bat_Dau,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Ngay_Ket_Thuc,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Mo_Ta,
                            x.SanPhamKhuyenMai.BangKhuyenMai.Trang_Thai
                        }
                    } : null
                })
                .ToListAsync();
        }

    }
}
