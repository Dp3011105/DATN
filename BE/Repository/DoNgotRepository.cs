using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class DoNgotRepository : IDoNgotRepository
    {
        private readonly MyDbContext _context;

        public DoNgotRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoNgotDTO>> GetAllAsync()
        {
            return await _context.DoNgot
                .Select(dn => new DoNgotDTO
                {
                    ID_DoNgot = dn.ID_DoNgot,
                    Muc_Do = dn.Muc_Do,
                    Ghi_Chu = dn.Ghi_Chu,
                    Trang_Thai = dn.Trang_Thai
                })
                .ToListAsync();
        }

        public async Task<DoNgotDTO> GetByIdAsync(int id)
        {
            return await _context.DoNgot
                .Where(dn => dn.ID_DoNgot == id)
                .Select(dn => new DoNgotDTO
                {
                    ID_DoNgot = dn.ID_DoNgot,
                    Muc_Do = dn.Muc_Do,
                    Ghi_Chu = dn.Ghi_Chu,
                    Trang_Thai = dn.Trang_Thai
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DoNgotDTO> CreateAsync(DoNgotDTO doNgotDTO)
        {
            var doNgot = new DoNgot
            {
                Muc_Do = doNgotDTO.Muc_Do,
                Ghi_Chu = doNgotDTO.Ghi_Chu,
                Trang_Thai = doNgotDTO.Trang_Thai
            };

            _context.DoNgot.Add(doNgot);
            await _context.SaveChangesAsync();

            return new DoNgotDTO
            {
                ID_DoNgot = doNgot.ID_DoNgot,
                Muc_Do = doNgot.Muc_Do,
                Ghi_Chu = doNgot.Ghi_Chu,
                Trang_Thai = doNgot.Trang_Thai
            };
        }

        public async Task<DoNgotDTO> UpdateAsync(int id, DoNgotDTO doNgotDTO)
        {
            var doNgot = await _context.DoNgot
                .FirstOrDefaultAsync(dn => dn.ID_DoNgot == id);

            if (doNgot == null)
            {
                return null;
            }

            doNgot.Muc_Do = doNgotDTO.Muc_Do;
            doNgot.Ghi_Chu = doNgotDTO.Ghi_Chu;
            doNgot.Trang_Thai = doNgotDTO.Trang_Thai;

            await _context.SaveChangesAsync();

            return new DoNgotDTO
            {
                ID_DoNgot = doNgot.ID_DoNgot,
                Muc_Do = doNgot.Muc_Do,
                Ghi_Chu = doNgot.Ghi_Chu,
                Trang_Thai = doNgot.Trang_Thai
            };
        }
    }
}
