namespace BE.Repository
{
    using System;
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

        // Trả về DTO cho danh sách
        public async Task<IEnumerable<HoaDonDTO>> GetAllAsync()
        {
            return await _context.Hoa_Don.Select(h => new HoaDonDTO
            {
                ID_Hoa_Don = h.ID_Hoa_Don,
                Ma_Hoa_Don = h.Ma_Hoa_Don,
                Ngay_Tao = h.Ngay_Tao,
                Tong_Tien = h.Tong_Tien,
                Trang_Thai = h.Trang_Thai
            }).ToListAsync();
        }

        // Lấy entity để dùng cho GetById/Update/Delete
        public async Task<HoaDon> GetByIdAsync(int id)
        {
            return await _context.Hoa_Don
                .Include(hd => hd.HoaDonChiTiets)
                .ThenInclude(cthd => cthd.SanPham)
                .FirstOrDefaultAsync(hd => hd.ID_Hoa_Don == id);
        }

        public async Task AddAsync(HoaDon entity)
        {
            _context.Hoa_Don.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, HoaDon entity)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"HoaDon with ID {id} not found.");

            // Cập nhật các trường cần thiết
            existing.Ma_Hoa_Don = entity.Ma_Hoa_Don;
            existing.Ngay_Tao = entity.Ngay_Tao;
            existing.Tong_Tien = entity.Tong_Tien;
            existing.Trang_Thai = entity.Trang_Thai;

            _context.Hoa_Don.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"HoaDon with ID {id} not found.");

            _context.Hoa_Don.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
