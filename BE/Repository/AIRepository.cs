using BE.Data;
using BE.DTOs;
using BE.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Repository.IRepository
{
    public class AIRepository : IAIRepository
    {
        private readonly MyDbContext _context;

        public AIRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSearchResult>> SearchProductsAsync(string query)
        {
            // Chuẩn hóa query: chuyển thành chữ thường, bỏ dấu (nếu cần)
            var keywords = query.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Lấy giờ Việt Nam (UTC+7)
            var vietnamTime = DateTime.UtcNow.AddHours(7);

            // Tìm kiếm sản phẩm
            var products = await _context.San_Pham
                .Include(sp => sp.SanPhamSizes)
                    .ThenInclude(sps => sps.Size)
                .Include(sp => sp.SanPhamToppings)
                    .ThenInclude(spt => spt.Topping)
                .Include(sp => sp.SanPhamKhuyenMais)
                    .ThenInclude(spkm => spkm.BangKhuyenMai)
                .Where(sp =>
                    keywords.Any(k =>
                        sp.Ten_San_Pham.ToLower().Contains(k) ||
                        (sp.Mo_Ta != null && sp.Mo_Ta.ToLower().Contains(k)) ||
                        sp.SanPhamSizes.Any(sps => sps.Size.SizeName.ToLower().Contains(k)) ||
                        sp.SanPhamToppings.Any(spt => spt.Topping.Ten.ToLower().Contains(k))
                    ))
                .Select(sp => new ProductSearchResult
                {
                    Ten_San_Pham = sp.Ten_San_Pham,
                    So_Luong = sp.So_Luong,
                    Sizes = sp.SanPhamSizes.Select(sps => sps.Size.SizeName).ToList(),
                    Toppings = sp.SanPhamToppings.Select(spt => spt.Topping.Ten).ToList(),
                    Co_Khuyen_Mai = sp.SanPhamKhuyenMais.Any(spkm =>
                        spkm.BangKhuyenMai.Ngay_Bat_Dau <= vietnamTime &&
                        spkm.BangKhuyenMai.Ngay_Ket_Thuc >= vietnamTime &&
                        spkm.BangKhuyenMai.Trang_Thai == true)
                })
                .Take(5)
                .ToListAsync();

            return products;
        }

        public async Task<List<string>> GetAllWithDetailsAsync()
        {
            return await _context.San_Pham
                .Select(sp => sp.Ten_San_Pham)
                .ToListAsync();
        }
    }
}