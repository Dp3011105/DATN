using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class Voucher
    {
        [Key]
        public int ID_Voucher { get; set; }

        [Required]
        [StringLength(50)]
        public string Ma_Voucher { get; set; }

        [StringLength(100)]
        public string Ten { get; set; }

        public int? So_Luong { get; set; }

        public decimal? Gia_Tri_Giam { get; set; }

        public decimal? So_Tien_Dat_Yeu_Cau { get; set; }

        public DateTime? Ngay_Bat_Dau { get; set; }

        public DateTime? Ngay_Ket_Thuc { get; set; }
       
        public bool? Trang_Thai { get; set; }

        public virtual ICollection<KhachHangVoucher> KhachHangVouchers { get; set; } = new List<KhachHangVoucher>();
        public virtual ICollection<HoaDonVoucher> HoaDonVouchers { get; set; } = new List<HoaDonVoucher>(); // Quan hệ mới
    }
}
