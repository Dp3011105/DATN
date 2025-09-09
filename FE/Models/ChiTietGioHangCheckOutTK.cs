using System.Text.Json.Serialization;

namespace FE.Models
{
    public class ChiTietGioHangCheckOutTK
    {
        public int ID_GioHang_ChiTiet { get; set; }
        public int ID_San_Pham { get; set; }
        public string Ten_San_Pham { get; set; }
        public decimal Gia_Hien_Thi { get; set; }
        public decimal Gia_Goc { get; set; }
        public KhuyenMaiCheckOutTK Khuyen_Mai { get; set; }
        public int So_Luong { get; set; }
        public int? ID_Size { get; set; }
        public string Ten_Size { get; set; }
        public string Ten_LuongDa { get; set; }
        public string Ten_DoNgot { get; set; }
        public string Ghi_Chu { get; set; }
        public string Hinh_Anh { get; set; }
        public List<ToppingCheckOutTK> Toppings { get; set; }
    }
}
