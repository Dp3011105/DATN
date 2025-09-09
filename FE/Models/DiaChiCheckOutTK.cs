using System.Text.Json.Serialization;

namespace FE.Models
{
    public class DiaChiCheckOutTK
    {
        [JsonPropertyName("iD_Dia_Chi")]
        public int ID_Dia_Chi { get; set; }

        [JsonPropertyName("dia_Chi")]
        public string Dia_Chi { get; set; }

        [JsonPropertyName("tinh_Thanh")]
        public string Tinh_Thanh { get; set; }

        [JsonPropertyName("ghi_Chu")]
        public string Ghi_Chu { get; set; }

        [JsonPropertyName("ghi_Chu_KhachHang")]
        public string Ghi_Chu_KhachHang { get; set; }

        [JsonPropertyName("trang_Thai")]
        public bool Trang_Thai { get; set; }
    }
}
