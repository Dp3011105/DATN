using BE.models;
using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class GanVoucherViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một khách hàng")]
        public List<int> ID_Khach_Hang { get; set; } = new List<int>();

        [Required(ErrorMessage = "Vui lòng chọn ít nhất một voucher")]
        public List<int> ID_Voucher { get; set; } = new List<int>();

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; } = 1;

        [Required(ErrorMessage = "Vui lòng nhập ghi chú")]
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string? GhiChu { get; set; }

        // Dữ liệu cho dropdown
        public List<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
        public List<Voucher> Vouchers { get; set; } = new List<Voucher>();

        // Danh sách KH VIP
        public List<KhachHang> TopVipKhachHangs { get; set; } = new List<KhachHang>();
    }
}