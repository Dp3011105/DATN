namespace BE.DTOs
{
    public class DiaChiBanHangDTO
    {
        public int ID_Dia_Chi { get; set; }
        public string Dia_Chi { get; set; }
        public string Tinh_Thanh { get; set; }
        public string Ghi_Chu { get; set; } // From DiaChi
        public string Ghi_Chu_KhachHang { get; set; } // From KhachHangDiaChi
    }
}
