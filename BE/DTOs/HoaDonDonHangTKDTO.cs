namespace BE.DTOs
{
    public class HoaDonDonHangTKDTO
    {
        public int ID_Hoa_Don { get; set; }
        public string Ma_Hoa_Don { get; set; }
        public DateTime Ngay_Tao { get; set; }
        public decimal Tong_Tien { get; set; }
        public decimal? Phi_Ship { get; set; }

        public string Trang_Thai { get; set; }
        public string Ghi_Chu { get; set; }
        public string Loai_Hoa_Don { get; set; }
        public string LyDoHuyDon { get; set; }
        public string LyDoDonHangCoVanDe { get; set; }
        public string Phuong_Thuc_Thanh_Toan { get; set; }
        public DiaChiDonHangTK DiaChi { get; set; }
        public List<HoaDonChiTietDonHangTKDTO> ChiTiets { get; set; } = new List<HoaDonChiTietDonHangTKDTO>();
        public List<VoucherDonHangTKDTO> Vouchers { get; set; } = new List<VoucherDonHangTKDTO>();
    }
}
