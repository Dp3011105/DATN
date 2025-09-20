namespace BE.DTOs
{
    public class HoaDonBanHangTKDTO
    {
        public int? ID_Khach_Hang { get; set; }
        public int? ID_Hinh_Thuc_Thanh_Toan { get; set; }
        public int? ID_Dia_Chi { get; set; }
        public int? ID_Voucher { get; set; }
        public decimal Phi_Ship { get; set; }  

        public decimal Tong_Tien { get; set; }
        public string Ghi_Chu { get; set; }
        public string Ma_Hoa_Don { get; set; }
        public List<HoaDonChiTietBanHangTKDTO> HoaDonChiTiets { get; set; }
    }
}
