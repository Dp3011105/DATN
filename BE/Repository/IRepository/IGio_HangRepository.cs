using BE.models;
namespace BE.Repository.IRepository
{
    public interface IGio_HangRepository
    {
        Task<Gio_Hang> GetCartByCustomerIdAsync(int idKhachHang);
        Task<SanPhamKhuyenMai> GetActivePromotionForProductAsync(int idSanPham, DateTime currentDate);

        Task AddCartAsync(Gio_Hang gioHang);
        Task UpdateCartAsync(Gio_Hang gioHang);
        Task AddCartDetailAsync(GioHang_ChiTiet gioHangChiTiet);
        Task<bool> DeleteCartDetailAsync(int idGioHangChiTiet);

    }
}
