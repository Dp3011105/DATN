using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ tên phải từ 2 đến 100 ký tự.")]
        public string Ho_Ten { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        [RegularExpression(@"^[a-zA-Z0-9_.]+$", ErrorMessage = "Tên đăng nhập chỉ chứa chữ cái, số, dấu chấm (.) hoặc dấu gạch dưới (_), không chứa khoảng trắng.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự.")]
        public string Ten_Nguoi_Dung { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 50 ký tự.")]
        public string Mat_Khau { get; set; }
        [Compare("Mat_Khau", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}

