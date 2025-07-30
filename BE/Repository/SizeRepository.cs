using BE.Data;
using BE.DTOs;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class SizeRepository : ISizeRepository
    {
        private readonly MyDbContext _context;

        public SizeRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<SizeDTO>> GetAllAsync()
        {
            return await _context.Size
                .Select(s => new SizeDTO
                {
                    ID_Size = s.ID_Size,
                    SizeName = s.SizeName,
                    Gia = s.Gia,
                    Trang_Thai = s.Trang_Thai
                })
                .ToListAsync();
        }

        public async Task<SizeDTO> GetByIdAsync(int id)
        {
            return await _context.Size
                .Where(s => s.ID_Size == id)
                .Select(s => new SizeDTO
                {
                    ID_Size = s.ID_Size,
                    SizeName = s.SizeName,
                    Gia = s.Gia,
                    Trang_Thai = s.Trang_Thai
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SizeDTO> CreateAsync(SizeDTO sizeDTO)
        {
            var size = new Size
            {
                SizeName = sizeDTO.SizeName,
                Gia = sizeDTO.Gia,
                Trang_Thai = sizeDTO.Trang_Thai
            };

            _context.Size.Add(size);
            await _context.SaveChangesAsync();

            return new SizeDTO
            {
                ID_Size = size.ID_Size,
                SizeName = size.SizeName,
                Gia = size.Gia,
                Trang_Thai = size.Trang_Thai
            };
        }

        public async Task<SizeDTO> UpdateAsync(int id, SizeDTO sizeDTO)
        {
            var size = await _context.Size
                .FirstOrDefaultAsync(s => s.ID_Size == id);

            if (size == null)
            {
                return null;
            }

            size.SizeName = sizeDTO.SizeName;
            size.Gia = sizeDTO.Gia;
            size.Trang_Thai = sizeDTO.Trang_Thai;

            await _context.SaveChangesAsync();

            return new SizeDTO
            {
                ID_Size = size.ID_Size,
                SizeName = size.SizeName,
                Gia = size.Gia,
                Trang_Thai = size.Trang_Thai
            };
        }
    }
}
