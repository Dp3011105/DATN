using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class GioHang_ChiTiet
    {
        [Key]
        public int ID_GioHang_ChiTiet { get; set; }
        public int ID_Gio_Hang { get; set; }
        public int ID_San_Pham { get; set; }
        public int? ID_Size { get; set; }
        public int? ID_SanPham_DoNgot { get; set; } // Khóa ngoại tới DoNgot
        public int? ID_LuongDa { get; set; }
        [Required]
        public string Ma_GioHang_ChiTiet { get; set; }
        [Required]
        public int So_Luong { get; set; }
        [StringLength(255)]
        public string Ghi_Chu { get; set; }
        [Required]
        public DateTime Ngay_Tao { get; set; } = DateTime.Now;

        public virtual Gio_Hang Gio_Hang { get; set; }
        public virtual SanPham San_Pham { get; set; }
        public virtual Size Size { get; set; }
        public virtual DoNgot DoNgot { get; set; } // Navigation property tới DoNgot
        public virtual LuongDa LuongDa { get; set; }
        public virtual List<GioHangChiTiet_Topping> GioHangChiTiet_Toppings { get; set; } = new List<GioHangChiTiet_Topping>();
    }
}
