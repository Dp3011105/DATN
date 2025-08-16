namespace FE.Models
{ // dùng cho chức năng ghép Tài khoản và Vai trò của nhân viên với nhau
    public class GanVaiTroRequest
    {
        public int ID_Tai_Khoan { get; set; }
        public List<int> Danh_Sach_ID_Vai_Tro { get; set; } = new List<int>();
    }
}
