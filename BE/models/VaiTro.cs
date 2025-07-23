using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class VaiTro
    {
        [Key]
        public int ID_Vai_Tro { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten_Vai_Tro { get; set; }

        public virtual ICollection<TaiKhoanVaiTro> TaiKhoanVaiTros { get; set; } = new List<TaiKhoanVaiTro>();
    }
}
