namespace BE.DTOs
{
    public class GioHangChiTietDTO
    {
        public int ID_GioHang_ChiTiet { get; set; }
        public int ID_Gio_Hang { get; set; }
        public int ID_San_Pham { get; set; }
        public int? ID_Size { get; set; }
        public int? ID_SanPham_DoNgot { get; set; }
        public int? ID_LuongDa { get; set; }
        public string Ma_GioHang_ChiTiet { get; set; }
        public int So_Luong { get; set; }
        public string Ghi_Chu { get; set; }
        public DateTime Ngay_Tao { get; set; }
        public SanPhamDTO San_Pham { get; set; }
        public SizeDTO Size { get; set; }
        public DoNgotDTO DoNgot { get; set; }
        public LuongDaDTO LuongDa { get; set; }
        public List<GioHangChiTietToppingDTO> GioHangChiTiet_Toppings { get; set; }
    }
}
