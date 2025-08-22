using System.Text.Json.Serialization;

namespace FE.Models
{
    public class Promotion // dùng cho chức năng Khuyễn mãi trong ứng dụng
    {
        [JsonPropertyName("iD_Khuyen_Mai")]
        public int ID_Promotion { get; set; }

        [JsonPropertyName("ten_Khuyen_Mai")]
        public string PromotionName { get; set; }

        [JsonPropertyName("ngay_Bat_Dau")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("ngay_Ket_Thuc")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("mo_Ta")]
        public string Description { get; set; }

        [JsonPropertyName("trang_Thai")]
        public bool Status { get; set; }
    }
}
