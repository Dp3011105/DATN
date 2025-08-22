namespace BE.DTOs
{
    public class CartDetailResponseDto
    {
        public int ID_GioHang_ChiTiet { get; set; }
        public int ID_Gio_Hang { get; set; }
        public int ID_San_Pham { get; set; }
        public int? ID_Size { get; set; }
        public int? ID_SanPham_DoNgot { get; set; }
        public int? ID_LuongDa { get; set; }
        public List<ToppingDTO> Toppings { get; set; }
        public int So_Luong { get; set; }
        public string Ghi_Chu { get; set; }
        public DateTime Ngay_Tao { get; set; }
    }
}
