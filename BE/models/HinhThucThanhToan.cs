using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class HinhThucThanhToan
    {
        public int ID_Hinh_Thuc_Thanh_Toan { get; set; }
        public string Phuong_Thuc_Thanh_Toan { get; set; }
        public string Cong_Thanh_Toan { get; set; }
        public string Ghi_Chu { get; set; }
        public bool Trang_Thai { get; set; }
        public List<HoaDon> HoaDons { get; set; } // Thêm navigation property
    }
}
