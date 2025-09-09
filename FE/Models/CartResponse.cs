using System.Text.Json.Serialization;

namespace FE.Models
{
    public class CartResponse
    {
        [JsonPropertyName("chi_Tiet_Gio_Hang")]
        public List<ChiTietGioHangCheckOutTK> Chi_Tiet_Gio_Hang { get; set; }
    }
}
