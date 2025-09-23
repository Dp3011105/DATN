using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class NhanVien
    {
        [Key]
        public int ID_Nhan_Vien { get; set; }


        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string Ho_Ten { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public bool? GioiTinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string So_Dien_Thoai { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Dia_Chi { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date, ErrorMessage = "Ngày sinh không hợp lệ")]
        public DateTime NamSinh { get; set; }

        [Required(ErrorMessage = "CCCD không được để trống")]
        [StringLength(20, ErrorMessage = "CCCD không được vượt quá 20 ký tự")]
        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "CCCD phải là số có từ 9 đến 12 chữ số")]
        public string CCCD { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public bool? Trang_Thai { get; set; }

        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string Ghi_Chu { get; set; }

        [StringLength(255)]
        public string? AnhNhanVien { get; set; }

        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    }
}
