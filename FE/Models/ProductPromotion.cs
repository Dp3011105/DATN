using System.Text.Json.Serialization;

namespace FE.Models
{
    public class ProductPromotion// dùng cho chức năng áp dụng khuyến mãi 
    {
        [JsonPropertyName("iD_San_Pham_Khuyen_Mai")]
        public int ID_ProductPromotion { get; set; }

        [JsonPropertyName("phan_Tram_Giam")]
        public int? DiscountPercentage { get; set; } // Đổi sang int? để chấp nhận null

        [JsonPropertyName("gia_Giam")]
        public decimal DiscountedPrice { get; set; }

        [JsonPropertyName("khuyenMai")]
        public Promotion? Promotion { get; set; }
    }
}
