using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class TaiKhoan
    {
        [Key]
        public int ID_Tai_Khoan { get; set; }

        public int? ID_Khach_Hang { get; set; }
        public int? ID_Nhan_Vien { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        [StringLength(50, ErrorMessage = "Tên người dùng không được vượt quá 50 ký tự")]
        [MinLength(4, ErrorMessage = "Tên người dùng phải có ít nhất 4 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên người dùng chỉ được chứa chữ cái (a-z, A-Z) và số (0-9), không được chứa dấu, khoảng trắng, hoặc ký tự đặc biệt")]
        public string Ten_Nguoi_Dung { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        public string Mat_Khau { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái tài khoản")]
        public bool Trang_Thai { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Ngay_Tao { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? Ngay_Cap_Nhat { get; set; }

        [ForeignKey("ID_Khach_Hang")]
        public virtual KhachHang? KhachHang { get; set; }

        [ForeignKey("ID_Nhan_Vien")]
        public virtual NhanVien? NhanVien { get; set; }

        public virtual ICollection<TaiKhoanVaiTro> TaiKhoanVaiTros { get; set; } = new List<TaiKhoanVaiTro>();
    }
}