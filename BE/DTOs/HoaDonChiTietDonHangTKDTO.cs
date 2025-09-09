namespace BE.DTOs
{
    public class HoaDonChiTietDonHangTKDTO
    {
        public int ID_HoaDon_ChiTiet { get; set; }
        public string Ten_San_Pham { get; set; }
        public string SizeName { get; set; }
        public string Muc_Do { get; set; }
        public string Ten_LuongDa { get; set; }
        public List<ToppingDonHangTKDTO> Toppings { get; set; } = new List<ToppingDonHangTKDTO>();
        public int So_Luong { get; set; }
        public decimal Tong_Tien { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
