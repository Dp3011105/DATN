using BE.models;

namespace FE.Models
{
    public class HoaDonChiTiet // dùng cho checkout model
    {
        public int iD_San_Pham { get; set; }
        public string ten_San_Pham { get; set; }
        public decimal gia_Hien_Thi { get; set; }
        public decimal gia_Goc { get; set; }
        public int so_Luong { get; set; }
        public string ten_Size { get; set; }
        public string ten_LuongDa { get; set; }
        public string ten_DoNgot { get; set; }
        public string ghi_Chu { get; set; }
        public int iD_Size { get; set; }
        public int iD_SanPham_DoNgot { get; set; }
        public int iD_LuongDa { get; set; }
        public string ma_Hoa_Don_ChiTiet { get; set; }
        public decimal gia_Them_Size { get; set; }
        public decimal gia_San_Pham { get; set; }
        public decimal tong_Tien { get; set; }
        public List<HoaDonChiTietTopping> hoaDonChiTietToppings { get; set; }
    }
}
