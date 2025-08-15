using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class AddToCartDTO  // dto này dùng để thêm dữ liệu vào giỏ hàng cho khách hàng  (Phước)
    {
        [Required]
        public int ID_Khach_Hang { get; set; }
        [Required]
        public int ID_San_Pham { get; set; }
        public int? ID_Size { get; set; }
        public int? ID_DoNgot { get; set; }
        public int? ID_LuongDa { get; set; }
        [Required]
        public int So_Luong { get; set; } = 1;
        [StringLength(255)]
        public string Ghi_Chu { get; set; }
        public List<int> Toppings { get; set; } = new List<int>();
    }
}
