using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class SanPhamTopping
    {
        [Key, Column(Order = 0)]
        public int ID_Topping { get; set; }

        [Key, Column(Order = 1)]
        public int ID_San_Pham { get; set; }

        [StringLength(100)]
        public string Mo_Ta { get; set; } // Giữ lại Mo_Ta để lưu tên topping

        [ForeignKey("ID_Topping")]
        public virtual Topping Topping { get; set; }

        [ForeignKey("ID_San_Pham")]
        public virtual SanPham SanPham { get; set; }
    }
}
