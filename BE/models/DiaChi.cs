using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class DiaChi
    {
        [Key]
        public int ID_Dia_Chi { get; set; }

        public string Dia_Chi { get; set; }
        public string Tinh_Thanh { get; set; } 
        public string? Ghi_Chu { get; set; }

        public bool? Trang_Thai { get; set; }
        public List<HoaDon> HoaDons { get; set; } // Add navigation property for HoaDon
        public virtual ICollection<KhachHangDiaChi> KhachHangDiaChis { get; set; } = new List<KhachHangDiaChi>();
    }
}
