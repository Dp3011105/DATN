namespace FE.Models
{
    public class LoginResponse
    {
        public int? ID_Khach_Hang { get; set; }
        public int? ID_Nhan_Vien { get; set; }
        public List<int> VaiTros { get; set; }
    }
}
