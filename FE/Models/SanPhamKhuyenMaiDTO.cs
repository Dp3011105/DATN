namespace FE.Models
{
    public class SanPhamKhuyenMaiDTO
    {
        public int ID_San_Pham { get; set; }
        public string Ten_San_Pham { get; set; }
        public decimal Gia { get; set; }
        public int So_Luong { get; set; }
        public string Hinh_Anh { get; set; }
        public string Mo_Ta { get; set; }
        public bool Trang_Thai { get; set; }
        public List<KhuyenMai> KhuyenMais { get; set; }
    }
}
