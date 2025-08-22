using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class SanPham
    {
        [Key]
        public int ID_San_Pham { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten_San_Pham { get; set; }

        public decimal? Gia { get; set; }
        public int So_Luong { get; set; }

        [StringLength(255)]
        public string Hinh_Anh { get; set; }

        [StringLength(255)]
        public string Mo_Ta { get; set; }

        public bool? Trang_Thai { get; set; }

        public virtual ICollection<SanPhamLuongDa> SanPhamLuongDas { get; set; } = new List<SanPhamLuongDa>();
        public virtual ICollection<SanPhamSize> SanPhamSizes { get; set; } = new List<SanPhamSize>();
        public virtual ICollection<SanPhamTopping> SanPhamToppings { get; set; } = new List<SanPhamTopping>();
        public virtual ICollection<GioHang_ChiTiet> GioHang_ChiTiets { get; set; } = new List<GioHang_ChiTiet>();
        public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();
        public virtual ICollection<SanPhamDoNgot> SanPhamDoNgots { get; set; } = new List<SanPhamDoNgot>();
        public virtual ICollection<SanPhamKhuyenMai> SanPhamKhuyenMais { get; set; } = new List<SanPhamKhuyenMai>();

    }
}
