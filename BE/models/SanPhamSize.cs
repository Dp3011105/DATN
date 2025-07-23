using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class SanPhamSize
    {
        [Key, Column(Order = 0)]
        public int ID_Size { get; set; }

        [Key, Column(Order = 1)]
        public int ID_San_Pham { get; set; }

        [StringLength(100)]
        public string Mo_Ta { get; set; } // Giữ lại Mo_Ta để lưu tên kích cỡ

        [StringLength(255)]
        public string Ghi_Chu { get; set; } // Giữ lại nếu bạn vẫn cần cột này

        [ForeignKey("ID_Size")]
        public virtual Size Size { get; set; }

        [ForeignKey("ID_San_Pham")]
        public virtual SanPham SanPham { get; set; }
    }
}
