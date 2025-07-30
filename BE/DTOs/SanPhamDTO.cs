using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class SanPhamDTO
    {
        public int ID_San_Pham { get; set; }
        [Required]
        [StringLength(100)]
        public string Ten_San_Pham { get; set; }
        public decimal? Gia { get; set; }
        public int? So_Luong { get; set; }
        [StringLength(255)]
        public string Hinh_Anh { get; set; }

        [StringLength(500)]
        public string Mo_Ta { get; set; }
        public bool? Trang_Thai { get; set; }
        public List<int> Sizes { get; set; } = new List<int>();
        public List<int> LuongDas { get; set; } = new List<int>();
        public List<int> DoNgots { get; set; } = new List<int>();
        public List<int> Toppings { get; set; } = new List<int>();
    }
}
