using System.Text.Json.Serialization;

namespace FE.Models
{
    public class VoucherCheckOutTK
    {
        [JsonPropertyName("iD_Voucher")]
        public int ID_Voucher { get; set; }

        [JsonPropertyName("ma_Voucher")]
        public string Ma_Voucher { get; set; }

        [JsonPropertyName("ten")]
        public string Ten { get; set; }

        [JsonPropertyName("so_Luong")]
        public int So_Luong { get; set; }

        [JsonPropertyName("gia_Tri_Giam")]
        public decimal Gia_Tri_Giam { get; set; }

        [JsonPropertyName("so_Tien_Dat_Yeu_Cau")]
        public decimal So_Tien_Dat_Yeu_Cau { get; set; }

        [JsonPropertyName("ngay_Bat_Dau")]
        public DateTime Ngay_Bat_Dau { get; set; }

        [JsonPropertyName("ngay_Ket_Thuc")]
        public DateTime Ngay_Ket_Thuc { get; set; }

        [JsonPropertyName("trang_Thai")]
        public bool Trang_Thai { get; set; }
    }
}
