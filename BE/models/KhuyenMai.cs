using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class KhuyenMai
    {
        [Key]
        public int ID_Khuyen_Mai { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten_Khuyen_Mai { get; set; } // Tên đợt khuyến mãi, ví dụ: "Giáng sinh 2025"

        [Required]
        public DateTime Ngay_Bat_Dau { get; set; } // Ngày bắt đầu khuyến mãi

        [Required]
        public DateTime Ngay_Ket_Thuc { get; set; } // Ngày kết thúc khuyến mãi

        [StringLength(255)]
        public string Mo_Ta { get; set; } // Mô tả khuyến mãi (tùy chọn)

        public bool Trang_Thai { get; set; } // Trạng thái khuyến mãi (kích hoạt hay không)

        public virtual ICollection<SanPhamKhuyenMai> SanPhamKhuyenMais { get; set; } = new List<SanPhamKhuyenMai>();
    }
}
