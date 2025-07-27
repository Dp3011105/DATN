using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class SanPhamLuongDa
    {
      
        public int ID_San_Pham { get; set; }
        public int ID_LuongDa { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual LuongDa LuongDa { get; set; }
    }
}
