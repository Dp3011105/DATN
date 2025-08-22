using System.Text.Json.Serialization;

namespace FE.Models
{
    public class Product// dùng cho chức năng Khuyến Mãi
    {
        [JsonPropertyName("iD_San_Pham")]
        public int ID_Product { get; set; }

        [JsonPropertyName("ten_San_Pham")]
        public string ProductName { get; set; }

        [JsonPropertyName("gia")]
        public decimal Price { get; set; }

        [JsonPropertyName("so_Luong")]
        public int Quantity { get; set; }

        [JsonPropertyName("hinh_Anh")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("trang_Thai")]
        public bool Status { get; set; }

        [JsonPropertyName("sanPhamKhuyenMai")]
        public ProductPromotion ProductPromotion { get; set; }
    }
}
