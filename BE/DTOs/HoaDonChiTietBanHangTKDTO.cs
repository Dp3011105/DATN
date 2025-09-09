namespace BE.DTOs
{
    public class HoaDonChiTietBanHangTKDTO
    {
        public int ID_San_Pham { get; set; }
        public string Ten_San_Pham { get; set; } // Thêm để chứa tên sản phẩm từ client
        public decimal Gia_Hien_Thi { get; set; } // Giá sau khuyến mãi
        public decimal Gia_Goc { get; set; } // Giá gốc
        public int So_Luong { get; set; }
        public string Ten_Size { get; set; } // Tên size (XXL(chubby), ...)
        public string Ten_LuongDa { get; set; } // Tên lượng đá (ít đá, ...)
        public string Ten_DoNgot { get; set; } // Tên độ ngọt (Đái ra đường, ...)
        public string Ghi_Chu { get; set; }
        public int? ID_Size { get; set; }
        public int? ID_SanPham_DoNgot { get; set; }
        public int? ID_LuongDa { get; set; }
        public string Ma_HoaDon_ChiTiet { get; set; }
        public decimal Gia_Them_Size { get; set; }
        public decimal Gia_San_Pham { get; set; }
        public decimal Tong_Tien { get; set; }
        public List<HoaDonChiTietToppingBanHangTKDTO> HoaDonChiTietToppings { get; set; }
    }

}
