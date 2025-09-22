using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class ProfileModel
    {
        public int ID_Khach_Hang { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        public string Ho_Ten { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } // Bỏ readonly

        [Display(Name = "Số điện thoại")]
        public string So_Dien_Thoai { get; set; }

        [Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; }

        [Display(Name = "Ghi chú")]
        public string Ghi_Chu { get; set; }

        public List<AddressModel> DiaChis { get; set; } = new List<AddressModel>();
    }

    public class AddressModel
    {
        public int ID_Dia_Chi { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [Display(Name = "Địa chỉ")]
        public string Dia_Chi { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string Tinh_Thanh { get; set; } = "Hà Nội";

        [Display(Name = "Ghi chú")]
        public string Ghi_Chu { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? Trang_Thai { get; set; }
    }

    public class ProfileUpdateModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string Ho_Ten { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } // Thêm email vào update model

        public string So_Dien_Thoai { get; set; }

        public bool GioiTinh { get; set; }

        public string Ghi_Chu { get; set; }
    }

    public class AddressCreateModel
    {
        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        public string Dia_Chi { get; set; }

        public string Tinh_Thanh { get; set; } = "Hà Nội";

        public string Ghi_Chu { get; set; }
    }
}