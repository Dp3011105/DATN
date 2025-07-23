using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class SanPhamLuongDa
    {
        [Key]
        public int ID_SanPhamLuongDa { get; set; }

        public int ID_San_Pham { get; set; }
        public int ID_LuongDa { get; set; }

        [ForeignKey("ID_San_Pham")]
        public virtual SanPham SanPham { get; set; }

        [ForeignKey("ID_LuongDa")]
        public virtual LuongDa LuongDa { get; set; }
    }
}
