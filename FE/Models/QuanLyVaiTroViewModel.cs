namespace FE.Models
{
    public class QuanLyVaiTroViewModel
    {
        public List<TaiKhoanNhanVien> NhanVienKhongVaiTro { get; set; } = new List<TaiKhoanNhanVien>();
        public List<TaiKhoanNhanVien> NhanVienCoVaiTro { get; set; } = new List<TaiKhoanNhanVien>();
        public List<VaiTro> VaiTros { get; set; } = new List<VaiTro>();
    }
}
