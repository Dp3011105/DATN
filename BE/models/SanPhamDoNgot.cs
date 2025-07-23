using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class SanPhamDoNgot
    {
        public int ID_San_Pham { get; set; }
        public int ID_DoNgot { get; set; }
        [StringLength(255)]
        public string Ghi_Chu { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual DoNgot DoNgot { get; set; }
    }
}
