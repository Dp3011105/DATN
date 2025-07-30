using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class LuongDaRepository : ILuongDaRepository
    {
        private readonly MyDbContext _context;

        public LuongDaRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<LuongDaDTO>> GetAllAsync()
        {
            return await _context.LuongDa
                .Select(ld => new LuongDaDTO
                {
                    ID_LuongDa = ld.ID_LuongDa,
                    Ten_LuongDa = ld.Ten_LuongDa,
                    Trang_Thai = ld.Trang_Thai
                })
                .ToListAsync();
        }

        public async Task<LuongDaDTO> GetByIdAsync(int id)
        {
            var luongDa = await _context.LuongDa
                .Where(ld => ld.ID_LuongDa == id)
                .Select(ld => new LuongDaDTO
                {
                    ID_LuongDa = ld.ID_LuongDa,
                    Ten_LuongDa = ld.Ten_LuongDa,
                    Trang_Thai = ld.Trang_Thai
                })
                .FirstOrDefaultAsync();

            return luongDa;
        }

        public async Task<LuongDaDTO> CreateAsync(LuongDaDTO luongDaDTO)
        {
            var luongDa = new LuongDa
            {
                // ID_LuongDa sẽ tự tăng
                Ten_LuongDa = luongDaDTO.Ten_LuongDa,
                Trang_Thai = luongDaDTO.Trang_Thai
            };

            _context.LuongDa.Add(luongDa);
            await _context.SaveChangesAsync();

            return new LuongDaDTO
            {
                ID_LuongDa = luongDa.ID_LuongDa,
                Ten_LuongDa = luongDa.Ten_LuongDa,
                Trang_Thai = luongDa.Trang_Thai
            };
        }

        public async Task<LuongDaDTO> UpdateAsync(int id, LuongDaDTO luongDaDTO)
        {
            var luongDa = await _context.LuongDa
                .FirstOrDefaultAsync(ld => ld.ID_LuongDa == id);

            if (luongDa == null)
            {
                return null;
            }

            luongDa.Ten_LuongDa = luongDaDTO.Ten_LuongDa;
            luongDa.Trang_Thai = luongDaDTO.Trang_Thai;

            await _context.SaveChangesAsync();

            return new LuongDaDTO
            {
                ID_LuongDa = luongDa.ID_LuongDa,
                Ten_LuongDa = luongDa.Ten_LuongDa,
                Trang_Thai = luongDa.Trang_Thai
            };
        }
    }
}