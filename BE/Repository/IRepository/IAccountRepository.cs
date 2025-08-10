using BE.models;

namespace BE.Repository.IRepository
{
    public interface IAccountRepository
    {
        // dùng cho chức năng đăng ký (phước)
        //Task<bool> UsernameExistsAsync(string username);
        //Task AddTaiKhoanAsync(TaiKhoan taiKhoan);
        //Task AddTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro);
        //Task<int> SaveChangesAsync();

        Task<bool> UsernameExistsAsync(string username);
        Task AddTaiKhoanAsync(TaiKhoan taiKhoan);
        Task AddTaiKhoanVaiTroAsync(TaiKhoanVaiTro taiKhoanVaiTro);
        Task<int> SaveChangesAsync();
        Task<TaiKhoan> GetTaiKhoanByUsernameAsync(string username);
        Task<List<int>> GetVaiTroIdsByTaiKhoanIdAsync(int taiKhoanId);
    }
}
