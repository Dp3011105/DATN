using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("ID_HoaDon_ChiTiet")]
        public virtual HoaDonChiTiet HoaDonChiTiet { get; set; }

        [ForeignKey("ID_Topping")]
        public virtual Topping Topping { get; set; }
    }
}
