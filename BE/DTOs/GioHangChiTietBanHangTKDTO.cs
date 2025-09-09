namespace BE.DTOs
{
    public class GioHangChiTietBanHangTKDTO
    {
        public int ID_GioHang_ChiTiet { get; set; }
        public int ID_San_Pham { get; set; }
        public string Ten_San_Pham { get; set; }
        public decimal Gia_Hien_Thi { get; set; }
        public decimal Gia_Goc { get; set; }
        public KhuyenMaiBanHangTKDTO Khuyen_Mai { get; set; }
        public int So_Luong { get; set; }
        public string Ten_Size { get; set; }
        public string Ten_LuongDa { get; set; }
        public string Ten_DoNgot { get; set; }
        public string Ghi_Chu { get; set; }
        public List<ToppingBanHangTKDTO> Toppings { get; set; }
    }
}
