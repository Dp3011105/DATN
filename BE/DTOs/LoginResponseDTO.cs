namespace BE.DTOs
{
    public class LoginResponseDTO
    {//DỮ LIỆU TRẢ VỀ NẾU NHẬP MẬT KHẨU ĐÚNG TRONG CHỨC NĂNG ĐĂNG NHẬP 
        public int? ID_Khach_Hang { get; set; }
        public int? ID_Nhan_Vien { get; set; }
        public List<int> VaiTros { get; set; }
    }
}
