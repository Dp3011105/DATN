using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class HoaDon
    {
        public int ID_Hoa_Don { get; set; }
        public int? ID_Khach_Hang { get; set; }
        public int? ID_Nhan_Vien { get; set; }
        public int? ID_Hinh_Thuc_Thanh_Toan { get; set; }
        public int? ID_Dia_Chi { get; set; }
        public int? ID_Phi_Ship { get; set; }
        public string Dia_Chi_Tu_Nhap { get; set; }
        public DateTime Ngay_Tao { get; set; }
        public decimal Tong_Tien { get; set; }
        public decimal? Phi_Ship { get; set; }
        public string Trang_Thai { get; set; } // Trạng thái được cứng trong code Chua_Xac_Nhan , Da_Xac_Nhan , Dang_Giao_Hang ,Hoan_Thanh, Do_Hang_Co_Van_De , Huy_Don
        public string Ghi_Chu { get; set; }
        public string Ma_Hoa_Don { get; set; }
        public string Loai_Hoa_Don { get; set; }
        public KhachHang KhachHang { get; set; }
        public NhanVien NhanVien { get; set; }
        public HinhThucThanhToan HinhThucThanhToan { get; set; }
        public DiaChi DiaChi { get; set; }
        public PhiShip PhiShip { get; set; }
        public string? LyDoHuyDon { get; set; }  // Cho phép null nếu không hủy đơn
        public string? LyDoDonHangCoVanDe { get; set; }  // Cho phép null nếu không hủy đơn

        public List<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public List<LichSuHoaDon> LichSuHoaDons { get; set; }
        public List<HoaDonVoucher> HoaDonVouchers { get; set; } // Quan hệ mới với bảng trung gian
    }
}
