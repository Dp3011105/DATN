using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class SanPhamKhuyenMai
    {
        [Key]
        public int ID_San_Pham_Khuyen_Mai { get; set; }

        [Required]
        public int ID_San_Pham { get; set; } // Khóa ngoại liên kết với SanPham

        [Required]
        public int ID_Khuyen_Mai { get; set; } // Khóa ngoại liên kết với BangKhuyenMai

        [Required]
        [Range(0, 100)]
        public decimal Phan_Tram_Giam { get; set; } // Phần trăm giảm giá, ví dụ: 20% thì lưu là 20

        public decimal? Gia_Giam { get; set; } // Giá sau khi giảm (tùy chọn, có thể tính toán)

        [ForeignKey("ID_San_Pham")]
        public virtual SanPham SanPham { get; set; }

        [ForeignKey("ID_Khuyen_Mai")]
        public virtual KhuyenMai BangKhuyenMai { get; set; }
    }
}
