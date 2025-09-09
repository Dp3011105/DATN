namespace BE.DTOs
{
    public class HoaDonChiTietToppingBanHangTKDTO
    {
        public int ID_Topping { get; set; }
        public string Ten_Topping { get; set; } // Thêm để chứa tên topping
        public int So_Luong { get; set; }
        public decimal Gia_Topping { get; set; }
    }
}
