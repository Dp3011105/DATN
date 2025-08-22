using System.Text.Json.Serialization;

namespace FE.Models
{
    public class AssignPromotionRequest// dùng cho áp dụng khuyến mãi cho sản phẩm
    {
        [JsonPropertyName("iD_Khuyen_Mai")]
        public int ID_Promotion { get; set; }

        [JsonPropertyName("iD_San_Phams")]
        public List<int> ID_Products { get; set; }

        [JsonPropertyName("phan_Tram_Giam")]
        public int DiscountPercentage { get; set; }
    }
}
