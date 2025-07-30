using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class LuongDaDTO
    {
        public int ID_LuongDa { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten_LuongDa { get; set; }

        public bool? Trang_Thai { get; set; }
    }
}
