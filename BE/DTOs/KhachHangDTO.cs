using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class KhachHangDTO
    {
        public int ID_Khach_Hang { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public string Ho_Ten { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có 10-11 số.")]
        public string So_Dien_Thoai { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc.")]
        public bool GioiTinh { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        public bool Trang_Thai { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string Ghi_Chu { get; set; }
    }
}
