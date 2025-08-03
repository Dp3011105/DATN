using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class ToppingDTO
    {
        public int ID_Topping { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten { get; set; }

        public decimal? Gia { get; set; }

        public int? So_Luong { get; set; }
        public string Hinh_Anh { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        public bool? Trang_Thai { get; set; }
    }
}
