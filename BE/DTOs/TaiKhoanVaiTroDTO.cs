namespace BE.DTOs
{
    public class TaiKhoanVaiTroDTO
    {
        public int ID_Vai_Tro { get; set; }
        public int ID_Tai_Khoan { get; set; }
        public string? TenVaiTro { get; set; }
        public string? TenTaiKhoan { get; set; }
        public string? Email { get; set; }
        public bool TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }
}
