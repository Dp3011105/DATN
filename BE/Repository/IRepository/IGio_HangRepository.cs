using BE.models;

namespace Repository.IRepository
{
    public interface IGio_HangRepository // cái này dùng cho chức năng lấy danh sách dữ liệu giỏ hàng của khách hàng (Phước)
    {
        Task<Gio_Hang> GetGioHangByKhachHangIdAsync(int idKhachHang); // dùng để lấy giỏ hàng của khách hàng theo ID nhé tềnh eo
        Task CreateGioHangAsync(Gio_Hang gioHang); // tạo mới 1 giỏ hàng nếu như khách hàng chưa có giỏ hàng nào , lười quá quên nhữ ra phài thực hiện từ lúc đăng ký tài khoản 
        Task AddGioHangChiTietAsync(GioHang_ChiTiet gioHangChiTiet); // thêm chi tiết giỏ hàng vào giỏ hàng của khách hàng 
        Task<bool> DeleteGioHangChiTietAsync(int idGioHangChiTiet, int idKhachHang); // xóa đi nếu khách hàng ko mún nựa 
    }
}