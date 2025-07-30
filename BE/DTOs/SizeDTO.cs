using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class SizeDTO
    {
        public int ID_Size { get; set; }

        [Required]
        [StringLength(20)]
        public string SizeName { get; set; }

        public decimal? Gia { get; set; }

        public bool? Trang_Thai { get; set; }
    }
}
