using BE.models;
using System.Text.Json.Serialization;
// dùng cho chức năng quản lý Vai trò của nhân viên nhé 
namespace FE.Models
{
    public class TaiKhoanNhanVien
    {
        [JsonPropertyName("iD_Tai_Khoan")]
        public int ID_Tai_Khoan { get; set; }

        [JsonPropertyName("ten_Dang_Nhap")]
        public string Ten_Dang_Nhap { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("trang_Thai")]
        public bool Trang_Thai { get; set; }

        [JsonPropertyName("ngay_Tao")]
        public DateTime Ngay_Tao { get; set; }

        [JsonPropertyName("ngay_Cap_Nhat")]
        public DateTime Ngay_Cap_Nhat { get; set; }

        [JsonPropertyName("iD_Nhan_Vien")]
        public int ID_Nhan_Vien { get; set; }

        [JsonPropertyName("ho_Ten")]
        public string Ho_Ten { get; set; }

        [JsonPropertyName("so_Dien_Thoai")]
        public string So_Dien_Thoai { get; set; }

        [JsonPropertyName("dia_Chi")]
        public string Dia_Chi { get; set; }

        [JsonPropertyName("nam_Sinh")]
        public DateTime Nam_Sinh { get; set; }

        [JsonPropertyName("cccd")]
        public string CCCD { get; set; }

        [JsonPropertyName("vai_Tros")]
        public List<VaiTro> Vai_Tros { get; set; } = new List<VaiTro>();
    }
}
