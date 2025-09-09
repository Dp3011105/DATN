using System.Text.Json.Serialization;

namespace FE.Models
{
    public class HinhThucThanhToanCheckOutTK
    {
        [JsonPropertyName("iD_Hinh_Thuc_Thanh_Toan")]
        public int ID_Hinh_Thuc_Thanh_Toan { get; set; }

        [JsonPropertyName("phuong_Thuc_Thanh_Toan")]
        public string Phuong_Thuc_Thanh_Toan { get; set; }

        [JsonPropertyName("cong_Thanh_Toan")]
        public string Cong_Thanh_Toan { get; set; }

        [JsonPropertyName("ghi_Chu")]
        public string Ghi_Chu { get; set; }

        [JsonPropertyName("trang_Thai")]
        public bool Trang_Thai { get; set; }
    }
}
