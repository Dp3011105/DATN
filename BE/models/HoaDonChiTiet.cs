using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class HoaDonChiTiet
    {
        [Key]
        public int ID_HoaDon_ChiTiet { get; set; }
        public int ID_Hoa_Don { get; set; }
        public int ID_San_Pham { get; set; }
        public int? ID_Size { get; set; }
        public int? ID_SanPham_DoNgot { get; set; } // Khóa ngoại tới DoNgot
        public int? ID_LuongDa { get; set; }
        [Required]
        public string Ma_HoaDon_ChiTiet { get; set; }
        public decimal Gia_Them_Size { get; set; }
        public decimal Gia_San_Pham { get; set; }
        public decimal Tong_Tien { get; set; }
        [Required]
        public int So_Luong { get; set; }
        [StringLength(255)]
        public string Ghi_Chu { get; set; }
        [Required]
        public DateTime Ngay_Tao { get; set; } = DateTime.Now;

        public virtual HoaDon HoaDon { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual Size Size { get; set; }
        public virtual DoNgot DoNgot { get; set; } // Navigation property tới DoNgot
        public virtual LuongDa LuongDa { get; set; }
        public virtual List<HoaDonChiTietTopping> HoaDonChiTietToppings { get; set; } = new List<HoaDonChiTietTopping>();
        public virtual List<HoaDonChiTietThue> HoaDonChiTietThues { get; set; } = new List<HoaDonChiTietThue>();
    }
}
