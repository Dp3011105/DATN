using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class LuongDa
    {
        [Key]
        public int ID_LuongDa { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten_LuongDa { get; set; }


        public bool? Trang_Thai { get; set; }

        public virtual ICollection<SanPhamLuongDa> SanPhamLuongDas { get; set; } = new List<SanPhamLuongDa>();
        public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();
        public virtual ICollection<GioHang_ChiTiet> GioHang_ChiTiets { get; set; } = new List<GioHang_ChiTiet>();
    }
}
