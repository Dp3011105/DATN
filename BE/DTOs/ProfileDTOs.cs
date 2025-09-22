using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class ProfileResponse
    {
        public int ID_Khach_Hang { get; set; }
        public string Ho_Ten { get; set; }
        public string Email { get; set; }
        public string So_Dien_Thoai { get; set; }
        public bool GioiTinh { get; set; }
        public string Ghi_Chu { get; set; }
        public List<AddressResponse> DiaChis { get; set; } = new List<AddressResponse>();
    }

    public class ProfileUpdateRequest
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string Ho_Ten { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } // Thêm email vào request

        public string So_Dien_Thoai { get; set; }
        public bool GioiTinh { get; set; }
        public string Ghi_Chu { get; set; }
    }

    public class AddressResponse
    {
        public int ID_Dia_Chi { get; set; }
        public string Dia_Chi { get; set; }
        public string Tinh_Thanh { get; set; }
        public string Ghi_Chu { get; set; }
        public bool? Trang_Thai { get; set; }
    }

    public class AddressCreateRequest
    {
        [Required]
        public string Dia_Chi { get; set; }
        public string Tinh_Thanh { get; set; } = "Hà Nội";
        public string Ghi_Chu { get; set; }
    }

    public class AddressUpdateRequest
    {
        [Required]
        public string Dia_Chi { get; set; }
        public string Tinh_Thanh { get; set; } = "Hà Nội";
        public string Ghi_Chu { get; set; }
        public bool? Trang_Thai { get; set; }
    }
}