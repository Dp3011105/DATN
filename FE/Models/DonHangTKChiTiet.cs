namespace FE.Models
{
    public class DonHangTKChiTiet
    {
        public int ID_HoaDon_ChiTiet { get; set; }
        public string Ten_San_Pham { get; set; }
        public string SizeName { get; set; }
        public string Muc_Do { get; set; }
        public string Ten_LuongDa { get; set; }
        public List<DonHangTKTopping> Toppings { get; set; }
        public int So_Luong { get; set; }
        public decimal Tong_Tien { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
