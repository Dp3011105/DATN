namespace BE.DTOs
{
    public class AddToCartRequestDto
    {
        public int ID_Khach_Hang { get; set; }
        public int ID_San_Pham { get; set; }
        public int? ID_Size { get; set; }
        public int? ID_SanPham_DoNgot { get; set; }
        public int? ID_LuongDa { get; set; }
        public List<int> ID_Toppings { get; set; }
        public int So_Luong { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
