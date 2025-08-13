namespace BE.DTOs
{
    public class GioHangDTO// dùng cho client để lấy dữ liệu giỏ hàng của khách hàng (Phước)
    {
        
        public int ID_Gio_Hang { get; set; }
        public int ID_Khach_Hang { get; set; }
        public DateTime Ngay_Tao { get; set; }
        public DateTime Ngay_Cap_Nhat { get; set; }
        public bool Trang_Thai { get; set; }
        public List<GioHangChiTietDTO> GioHang_ChiTiets { get; set; }
    }
}

