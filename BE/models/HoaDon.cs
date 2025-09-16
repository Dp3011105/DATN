using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BE.models
{
    public class HoaDon
    {
        [Key]
        public int ID_Hoa_Don { get; set; }

        // FK client gửi
        public int? ID_Khach_Hang { get; set; }
        public int? ID_Nhan_Vien { get; set; }
        public int? ID_Hinh_Thuc_Thanh_Toan { get; set; }
        public int? ID_Dia_Chi { get; set; }
        public int? ID_Phi_Ship { get; set; }

        // Thông tin đơn
        public string? Dia_Chi_Tu_Nhap { get; set; }
        public DateTime Ngay_Tao { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tong_Tien { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Phi_Ship { get; set; }

        // Mặc định
        public string Trang_Thai { get; set; } = "Chua_Xac_Nhan";
        public string? Ghi_Chu { get; set; }
        [Required] public string Ma_Hoa_Don { get; set; } = null!;
        [Required] public string Loai_Hoa_Don { get; set; } = "TaiQuay";

        public string? LyDoHuyDon { get; set; }
        public string? LyDoDonHangCoVanDe { get; set; }

        // ===== Navigation properties =====
        // ⭐ BỎ JsonIgnore để FE nhận được tên KH, SĐT và địa chỉ
        [ForeignKey(nameof(ID_Khach_Hang))]
        [ValidateNever] public virtual KhachHang? KhachHang { get; set; }

        [ForeignKey(nameof(ID_Dia_Chi))]
        [ValidateNever] public virtual DiaChi? DiaChi { get; set; }

        // Các nav lớn khác vẫn ignore để payload gọn
        [ForeignKey(nameof(ID_Nhan_Vien))]
        [JsonIgnore, ValidateNever] public virtual NhanVien? NhanVien { get; set; }

        [ForeignKey(nameof(ID_Hinh_Thuc_Thanh_Toan))]
        [JsonIgnore, ValidateNever] public virtual HinhThucThanhToan? HinhThucThanhToan { get; set; }

        // NHẬN chi tiết từ JSON: KHÔNG JsonIgnore
        [ValidateNever] public virtual List<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new();

        [JsonIgnore, ValidateNever] public virtual List<LichSuHoaDon> LichSuHoaDons { get; set; } = new();
        [JsonIgnore, ValidateNever] public virtual List<HoaDonVoucher> HoaDonVouchers { get; set; } = new();
    }
}
