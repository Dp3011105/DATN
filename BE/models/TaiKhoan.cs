using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class TaiKhoan
    {
        [Key]
        public int ID_Tai_Khoan { get; set; }

        public int? ID_Khach_Hang { get; set; }

        public int? ID_Nhan_Vien { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten_Nguoi_Dung { get; set; }

        [Required]
        [StringLength(100)]
        public string Mat_Khau { get; set; }

        public bool? Trang_Thai { get; set; }

        [ForeignKey("ID_Khach_Hang")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("ID_Nhan_Vien")]
        public virtual NhanVien NhanVien { get; set; }

        public virtual ICollection<TaiKhoanVaiTro> TaiKhoanVaiTros { get; set; } = new List<TaiKhoanVaiTro>();
    }
}
