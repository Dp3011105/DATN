namespace BE.DTOs
{
    public class HoaDonBanHangCKDTO
    {
        public int? ID_Khach_Hang { get; set; }
        public int? ID_Hinh_Thuc_Thanh_Toan { get; set; }
        public string? Dia_Chi_Tu_Nhap { get; set; }
        public int? ID_Voucher { get; set; }
        public decimal Tong_Tien { get; set; }
        public string Ghi_Chu { get; set; }
        public string Ma_Hoa_Don { get; set; }
        public List<HoaDonChiTietBanHangTKDTO> HoaDonChiTiets { get; set; }
    }
}
