using BE.models;

namespace BE.Repository.IRepository
{
    public interface ICustomerRepository
    {
        // dùng cho chức năng đăng ký ( Phước)
        Task<bool> EmailExistsAsync(string email);
        Task AddKhachHangAsync(KhachHang khachHang);
        Task<int> SaveChangesAsync();
    }
}
