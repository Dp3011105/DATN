namespace BE.models
{
    public class HoaDonChiTietThue
    {
        public int ID_HoaDon_ChiTiet { get; set; }
        public int ID_Thue { get; set; }
        public string Ghi_Chu { get; set; }
        public bool Trang_Thai { get; set; }
        public HoaDonChiTiet HoaDonChiTiet { get; set; }
        public Thue Thue { get; set; }
    }
}
