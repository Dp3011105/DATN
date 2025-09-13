using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BE.models
{
    public class HoaDonChiTietTopping
    {
        [Key]
        public int ID { get; set; }

        public int ID_HoaDon_ChiTiet { get; set; }

        public int ID_Topping { get; set; }

        public int? So_Luong { get; set; }

        public decimal? Gia_Topping { get; set; }


        [ForeignKey(nameof(ID_HoaDon_ChiTiet))]
        [JsonIgnore, ValidateNever]
        public virtual HoaDonChiTiet HoaDonChiTiet { get; set; }

        [ForeignKey(nameof(ID_Topping))]
        [JsonIgnore, ValidateNever]
        public virtual Topping Topping { get; set; }

    }
}
