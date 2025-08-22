namespace BE.DTOs
{
    public class KhuyenMaiDTO
    {
        public int ID_Khuyen_Mai { get; set; }
        public string Ten_Khuyen_Mai { get; set; }
        public DateTime Ngay_Bat_Dau { get; set; }
        public DateTime Ngay_Ket_Thuc { get; set; }
        public string Mo_Ta { get; set; }
        public bool Trang_Thai { get; set; }
    }
}
