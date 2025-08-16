namespace BE.DTOs
{
    public class NhanVienVoiTaiKhoan
    {
        public int ID_Tai_Khoan { get; set; }
        public string Ten_Dang_Nhap { get; set; }
        public string Email { get; set; }
        public bool Trang_Thai { get; set; }
        public DateTime Ngay_Tao { get; set; }
        public DateTime? Ngay_Cap_Nhat { get; set; }
        public int ID_Nhan_Vien { get; set; }
        public string Ho_Ten { get; set; }
        public string So_Dien_Thoai { get; set; }
        public string Dia_Chi { get; set; }
        public DateTime Nam_Sinh { get; set; }
        public string CCCD { get; set; }
        public List<VaiTroDto> Vai_Tros { get; set; }
    }
}
