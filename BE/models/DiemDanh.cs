using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class DiemDanh
    {
        [Key]
        public int ID_Diem_Danh { get; set; }

        public int NhanVien_ID { get; set; }

        [StringLength(100)]
        public string Vi_Tri { get; set; }

        [StringLength(50)]
        public string Ca { get; set; }

        public DateTime? Ngay_Diem_Danh { get; set; }

        public TimeSpan? Gio_Bat_Dau { get; set; }

        public TimeSpan? Gio_Ket_Thuc { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        [ForeignKey("NhanVien_ID")]
        public virtual NhanVien NhanVien { get; set; }
    }
}
