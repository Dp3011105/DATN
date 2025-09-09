using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class CreateDiaChiBanHangDTO
    {
        [Required]
        public string Dia_Chi { get; set; }
        [Required]
        public string Tinh_Thanh { get; set; }
        public string Ghi_Chu { get; set; }
        public string Ghi_Chu_KhachHang { get; set; }
    }
}
