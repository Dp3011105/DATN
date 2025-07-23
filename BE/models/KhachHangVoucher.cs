using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class KhachHangVoucher
    {
        [Key, Column(Order = 0)]
        public int ID_Khach_Hang { get; set; }

        [Key, Column(Order = 1)]
        public int ID_Voucher { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        public bool? Trang_Thai { get; set; }

        [ForeignKey("ID_Khach_Hang")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("ID_Voucher")]
        public virtual Voucher Voucher { get; set; }
    }
}
