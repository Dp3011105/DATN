namespace FE.Models
{
    public class SanPhamKhuyenMai
    {
        public int ID_San_Pham_Khuyen_Mai { get; set; } // ID của bản ghi khuyến mãi sản phẩm
        public int Phan_Tram_Giam { get; set; } // Phần trăm giảm giá
        public decimal Gia_Giam { get; set; } // Giá sau khi giảm
        public KhuyenMai KhuyenMai { get; set; } // Thông tin khuyến mãi liên quan
    }
}
