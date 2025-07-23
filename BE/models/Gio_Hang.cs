namespace BE.models
{
    public class Gio_Hang
    {
        public int ID_Gio_Hang { get; set; }
        public int ID_Khach_Hang { get; set; }
        public DateTime Ngay_Tao { get; set; }
        public DateTime Ngay_Cap_Nhat { get; set; }
        public bool Trang_Thai { get; set; }
        public KhachHang Khach_Hang { get; set; }
        public List<GioHang_ChiTiet> GioHang_ChiTiets { get; set; }
    }
}
