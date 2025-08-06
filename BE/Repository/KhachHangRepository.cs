using BE.DTOs;
using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly MyDbContext _context;

        public KhachHangRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddKhachHang(KhachHangDTO dto)
        {
            try
            {
                var entity = new KhachHang
                {
                    ID_Khach_Hang = dto.ID_Khach_Hang,
                    Ho_Ten = dto.Ho_Ten,
                    Email = dto.Email,
                    GioiTinh = dto.GioiTinh,
                    So_Dien_Thoai = dto.So_Dien_Thoai,
                    Trang_Thai = dto.Trang_Thai,
                    Ghi_Chu = dto.Ghi_Chu
                };
                _context.Khach_Hang.Add(entity);
                await _context.SaveChangesAsync();
                dto.ID_Khach_Hang = entity.ID_Khach_Hang; // Update DTO with generated ID
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the customer.", ex);
            }
        }

        public async Task DeleteKhachHang(int id)
        {
            try
            {
                var customer = await _context.Khach_Hang
                    .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == id);
                if (customer == null)
                {
                    throw new KeyNotFoundException("Customer not found.");
                }
                _context.Khach_Hang.Remove(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the customer.", ex);
            }
        }

        public async Task<IEnumerable<KhachHangDTO>> GetAllKhachHang()
        {
            return await _context.Khach_Hang
                .Select(kh => new KhachHangDTO
                {
                    ID_Khach_Hang = kh.ID_Khach_Hang,
                    Ho_Ten = kh.Ho_Ten,
                    Email = kh.Email,
                    GioiTinh = kh.GioiTinh,
                    So_Dien_Thoai = kh.So_Dien_Thoai,
                    Trang_Thai = kh.Trang_Thai,
                    Ghi_Chu = kh.Ghi_Chu
                })
                .ToListAsync();
        }

        public async Task<KhachHangDTO> GetKhachHangById(int id)
        {
            var customer = await _context.Khach_Hang
                .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == id);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            return new KhachHangDTO
            {
                ID_Khach_Hang = customer.ID_Khach_Hang,
                Ho_Ten = customer.Ho_Ten,
                Email = customer.Email,
                GioiTinh = customer.GioiTinh,
                So_Dien_Thoai = customer.So_Dien_Thoai,
                Trang_Thai = customer.Trang_Thai,
                Ghi_Chu = customer.Ghi_Chu
            };
        }

        public async Task UpdateKhachHang(int id, KhachHangDTO dto)
        {
            try
            {
                var existingCustomer = await _context.Khach_Hang
                    .FirstOrDefaultAsync(kh => kh.ID_Khach_Hang == id);
                if (existingCustomer == null)
                {
                    throw new KeyNotFoundException("Customer not found.");
                }

                existingCustomer.Ho_Ten = dto.Ho_Ten;
                existingCustomer.Email = dto.Email;
                existingCustomer.GioiTinh = dto.GioiTinh;
                existingCustomer.So_Dien_Thoai = dto.So_Dien_Thoai;
                existingCustomer.Trang_Thai = dto.Trang_Thai;
                existingCustomer.Ghi_Chu = dto.Ghi_Chu;

                _context.Khach_Hang.Update(existingCustomer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the customer.", ex);
            }
        }
    }
}