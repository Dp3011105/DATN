using BE.models;

namespace FE.Models
{
    public class GioHangChiTiet
    {
        public int ID_GioHang_ChiTiet { get; set; }
        public int ID_Gio_Hang { get; set; }
        public int ID_San_Pham { get; set; }
        public int? ID_Size { get; set; }
        public int? ID_SanPham_DoNgot { get; set; } // Có lẽ là ID_DoNgot
        public int? ID_LuongDa { get; set; }
        public string Ma_GioHang_ChiTiet { get; set; }
        public int So_Luong { get; set; }
        public string Ghi_Chu { get; set; }
        public DateTime Ngay_Tao { get; set; }
        public SanPham San_Pham { get; set; }
        public Size Size { get; set; }
        public DoNgot DoNgot { get; set; }
        public LuongDa LuongDa { get; set; }
        public List<GioHangChiTiet_Topping> GioHangChiTiet_Toppings { get; set; } = new List<GioHangChiTiet_Topping>();
    }
}
