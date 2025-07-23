using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class Thue
    {
        public int ID_Thue { get; set; }
        public string Ten_Thue { get; set; }
        public decimal Ty_Le { get; set; }
        public string Mo_Ta { get; set; }
        public bool Trang_Thai { get; set; }
        public List<HoaDonChiTietThue> HoaDonChiTietThues { get; set; } // Sử dụng BE.Data.HoaDonChiTietThue
    }
}
