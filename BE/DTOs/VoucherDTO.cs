namespace BE.DTOs
{
    public class VoucherDTO // Phwớc làm phần này   
    {
        public int ID_Voucher { get; set; }
        public string Ma_Voucher { get; set; }
        public string Ten { get; set; }
        public int? So_Luong { get; set; }
        public decimal? Gia_Tri_Giam { get; set; }
        public decimal? So_Tien_Dat_Yeu_Cau { get; set; }
        public DateTime? Ngay_Bat_Dau { get; set; }
        public DateTime? Ngay_Ket_Thuc { get; set; }
        public bool? Trang_Thai { get; set; }
    }
}
