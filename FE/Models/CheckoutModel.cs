
namespace FE.Models
{
    public class CheckoutModel
    {
        public int iD_Khach_Hang { get; set; }
        public int iD_Hinh_Thuc_Thanh_Toan { get; set; }
        public int iD_Dia_Chi { get; set; }
        public int? iD_Voucher { get; set; } // Nullable to support null when no voucher is selected
        public decimal tong_Tien { get; set; }
        public string ghi_Chu { get; set; }
        public string ma_Hoa_Don { get; set; }
        public List<HoaDonChiTiet> hoaDonChiTiets { get; set; }
    }
}
