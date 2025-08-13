using BE.models;

namespace FE.Models
{
    public class GioHang
    {
        public int ID_Gio_Hang { get; set; }
        public int ID_Khach_Hang { get; set; }
        public DateTime Ngay_Tao { get; set; }
        public DateTime? Ngay_Cap_Nhat { get; set; }
        public bool Trang_Thai { get; set; }
        public List<GioHangChiTiet> GioHang_ChiTiets { get; set; } = new List<GioHangChiTiet>();
    }
}
