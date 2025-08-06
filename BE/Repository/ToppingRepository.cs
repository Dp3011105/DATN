using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class ToppingRepository : IToppingRepository
    {
        private readonly MyDbContext _context;

        public ToppingRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToppingDTO>> GetAllAsync()
        {
            return await _context.Topping
                .Select(t => new ToppingDTO
                {
                    ID_Topping = t.ID_Topping,
                    Ten = t.Ten,
                    Gia = t.Gia,
                    So_Luong = t.So_Luong,
                    Hinh_Anh = t.Hinh_Anh,
                    Ghi_Chu = t.Ghi_Chu,
                    Trang_Thai = t.Trang_Thai
                })
                .ToListAsync();
        }

        public async Task<ToppingDTO> GetByIdAsync(int id)
        {
            return await _context.Topping
                .Where(t => t.ID_Topping == id)
                .Select(t => new ToppingDTO
                {
                    ID_Topping = t.ID_Topping,
                    Ten = t.Ten,
                    Gia = t.Gia,
                    So_Luong = t.So_Luong,
                    Hinh_Anh = t.Hinh_Anh,
                    Ghi_Chu = t.Ghi_Chu,
                    Trang_Thai = t.Trang_Thai
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ToppingDTO> CreateAsync(ToppingDTO toppingDTO)
        {
            var topping = new Topping
            {
                Ten = toppingDTO.Ten,
                Gia = toppingDTO.Gia,
                So_Luong = toppingDTO.So_Luong,
                Hinh_Anh = toppingDTO.Hinh_Anh,
                Ghi_Chu = toppingDTO.Ghi_Chu,
                Trang_Thai = toppingDTO.Trang_Thai
            };

            _context.Topping.Add(topping);
            await _context.SaveChangesAsync();

            return new ToppingDTO
            {
                ID_Topping = topping.ID_Topping,
                Ten = topping.Ten,
                Gia = topping.Gia,
                So_Luong = topping.So_Luong,
                Hinh_Anh = topping.Hinh_Anh,
                Ghi_Chu = topping.Ghi_Chu,
                Trang_Thai = topping.Trang_Thai
            };
        }

        public async Task<ToppingDTO> UpdateAsync(int id, ToppingDTO toppingDTO)
        {
            var topping = await _context.Topping
                .FirstOrDefaultAsync(t => t.ID_Topping == id);

            if (topping == null)
            {
                return null;
            }

            topping.Ten = toppingDTO.Ten;
            topping.Gia = toppingDTO.Gia;
            topping.So_Luong = toppingDTO.So_Luong;
            topping.Hinh_Anh = toppingDTO.Hinh_Anh;
            topping.Ghi_Chu = toppingDTO.Ghi_Chu;
            topping.Trang_Thai = toppingDTO.Trang_Thai;

            await _context.SaveChangesAsync();

            return new ToppingDTO
            {
                ID_Topping = topping.ID_Topping,
                Ten = topping.Ten,
                Gia = topping.Gia,
                So_Luong = topping.So_Luong,
                Hinh_Anh = topping.Hinh_Anh,
                Ghi_Chu = topping.Ghi_Chu,
                Trang_Thai = topping.Trang_Thai
            };
        }
    }
}