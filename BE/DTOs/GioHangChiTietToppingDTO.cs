namespace BE.DTOs
{
    public class GioHangChiTietToppingDTO
    {
        public int ID_GioHangChiTiet_Topping { get; set; }
        public int ID_GioHang_ChiTiet { get; set; }
        public int ID_Topping { get; set; }
        public ToppingDTO Topping { get; set; }
    }
}
