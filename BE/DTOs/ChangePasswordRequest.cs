namespace BE.DTOs
{
    public class ChangePasswordRequest
    {
        public int IdKhachHang { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
