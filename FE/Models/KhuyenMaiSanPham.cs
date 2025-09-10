using System.Text.Json.Serialization;

namespace FE.Models
{
    public class KhuyenMaiSanPham
    {
        [JsonPropertyName("iD_Khuyen_Mai")]
        public int ID_Khuyen_Mai { get; set; }
        [JsonPropertyName("ten_Khuyen_Mai")]
        public string Ten_Khuyen_Mai { get; set; }
        [JsonPropertyName("ngay_Bat_Dau")]
        public DateTime Ngay_Bat_Dau { get; set; }
        [JsonPropertyName("ngay_Ket_Thuc")]
        public DateTime Ngay_Ket_Thuc { get; set; }
        [JsonPropertyName("mo_Ta")]
        public string Mo_Ta { get; set; }
        [JsonPropertyName("trang_Thai")]
        public bool Trang_Thai { get; set; }
        [JsonPropertyName("gia_Giam")]
        public decimal? Gia_Giam { get; set; }
    }
}