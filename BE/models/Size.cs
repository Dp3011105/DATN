using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class Size
    {
        [Key]
        public int ID_Size { get; set; }

        [StringLength(20)]
        public string SizeName { get; set; }

        public decimal? Gia { get; set; }

        public bool? Trang_Thai { get; set; }


        // Add this:
        public List<GioHang_ChiTiet> GioHang_ChiTiets { get; set; }

        public virtual ICollection<SanPhamSize> SanPhamSizes { get; set; } = new List<SanPhamSize>();
        public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();
    }
}
