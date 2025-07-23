using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class LichSuHoaDon
    {
        [Key]
        public int ID_Lich_Su_Hoa_Don { get; set; }

        public int ID_Hoa_Don { get; set; }

        public DateTime? Ngay_Tao { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string Nguoi_Tao { get; set; }

        public DateTime? Ngay_Cap_Nhat { get; set; }

        [StringLength(100)]
        public string Nguoi_Cap_Nhat { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        [StringLength(100)]
        public string Hanh_Dong { get; set; }

        [ForeignKey("ID_Hoa_Don")]
        public virtual HoaDon HoaDon { get; set; }
    }
}
