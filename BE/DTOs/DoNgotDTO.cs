using System.ComponentModel.DataAnnotations;

namespace BE.DTOs
{
    public class DoNgotDTO
    {
        public int ID_DoNgot { get; set; }

        [Required]
        [StringLength(50)]
        public string Muc_Do { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        public bool Trang_Thai { get; set; }
    }
}
