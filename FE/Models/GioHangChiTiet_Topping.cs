namespace FE.Models
{
    public class GioHangChiTiet_Topping
    {
        public int ID_GioHangChiTiet_Topping { get; set; }
        public int ID_GioHang_ChiTiet { get; set; }
        public int ID_Topping { get; set; }
        public Topping Topping { get; set; }
    }
}
