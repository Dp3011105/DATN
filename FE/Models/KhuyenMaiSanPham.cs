using Newtonsoft.Json;

namespace FE.Models
{
    public class KhuyenMaiSanPham
    {
        [JsonProperty("iD_Khuyen_Mai")]
        public int ID_KhuyenMai { get; set; }
        [JsonProperty("ten_Khuyen_Mai")]
        public string Ten_Khuyen_Mai { get; set; }
        [JsonProperty("ngay_Bat_Dau")]
        public DateTime Ngay_Bat_Dau { get; set; }
        [JsonProperty("ngay_Ket_Thuc")]
        public DateTime Ngay_Ket_Thuc { get; set; }
        [JsonProperty("mo_Ta")]
        public string Mo_Ta { get; set; }
        [JsonProperty("trang_Thai")]
        public bool Trang_Thai { get; set; }
        public decimal? Gia_Giam { get; set; } // Có thể null, dùng trong SanPham.KhuyenMais
    }

}
