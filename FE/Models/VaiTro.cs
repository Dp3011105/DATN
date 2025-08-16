using System.Text.Json.Serialization;

namespace FE.Models
{ // dùng cho chức năng ghép Tài khoản và Vai trò của nhân viên với nhau
    public class VaiTro
    {
        [JsonPropertyName("iD_Vai_Tro")]
        public int ID_Vai_Tro { get; set; }

        [JsonPropertyName("ten_Vai_Tro")]
        public string Ten_Vai_Tro { get; set; }
    }
}
