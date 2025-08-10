﻿using BE.Data;
using BE.models;
using Microsoft.EntityFrameworkCore;

public class HoaDonRepository : IHoaDonRepository
{
    private readonly MyDbContext _context;
    public HoaDonRepository(MyDbContext context) => _context = context;

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


        public async Task AddAsync(HoaDon entity)
        {
            try
            {
                _context.Hoa_Don.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while adding the HoaDon.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var hoaDon = await GetByIdAsync(id);
                if (hoaDon == null)
                {
                    throw new KeyNotFoundException($"HoaDon with ID {id} not found.");
                }
                _context.Hoa_Don.Remove(hoaDon);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while deleting the HoaDon.", ex);
            }
        }

        public async Task<IEnumerable<HoaDon>> GetAllAsync()
        {
            return await _context.Hoa_Don
                .Include(hd => hd.HoaDonChiTiets)
                .ThenInclude(cthd => cthd.SanPham)
                .ToListAsync();
        }

        public async Task<HoaDon> GetByIdAsync(int id)
        {
            return await _context.Hoa_Don
                .Include(hd => hd.HoaDonChiTiets)
                .ThenInclude(cthd => cthd.SanPham)
                .FirstOrDefaultAsync(hd => hd.ID_Hoa_Don == id);
        }

        public async Task UpdateAsync(int id, HoaDon entity)
        {
            try
            {
                var existingHoaDon = await GetByIdAsync(id);
                if (existingHoaDon == null)
                {
                    throw new KeyNotFoundException($"HoaDon with ID {id} not found.");
                }
                existingHoaDon.Trang_Thai = entity.Trang_Thai; // Update other properties as needed
                _context.Hoa_Don.Update(existingHoaDon);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while updating the HoaDon.", ex);
            }
        }
    }
}
