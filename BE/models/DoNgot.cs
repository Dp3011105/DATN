using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class DoNgot
    {
        [Key]
        public int ID_DoNgot { get; set; }

        [StringLength(50)]
        public string Muc_Do { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        public bool Trang_Thai { get; set; }

        public virtual ICollection<GioHang_ChiTiet> GioHang_ChiTiets { get; set; } = new List<GioHang_ChiTiet>();
        public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();
        public virtual ICollection<SanPhamDoNgot> SanPhamDoNgots { get; set; } = new List<SanPhamDoNgot>();
    }
}
