using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class HoaDonVoucher
    {
        [Key]
        public int ID_HoaDonVoucher { get; set; }
        public int ID_Hoa_Don { get; set; }
        public int ID_Voucher { get; set; }
        public decimal Gia_Tri_Giam { get; set; }
        public HoaDon HoaDon { get; set; }
        public Voucher Voucher { get; set; }
    }
}
