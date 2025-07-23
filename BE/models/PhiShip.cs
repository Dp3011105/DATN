using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class PhiShip
    {
        [Key]
        public int ID_Phi_Ship { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Muc_Phi { get; set; }

        [StringLength(255)]
        public string Mo_Ta { get; set; }

        public bool? Trang_Thai { get; set; }

        // Thuộc tính mới: khu vực áp dụng
        [StringLength(100)]
        public string Tinh_Thanh_Ap_Dung { get; set; }   // Ví dụ: "Hà Nội", "Bắc Ninh", "Hưng Yên"

        public int? Khoang_Cach_Toi_Da_Km { get; set; }


        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }

}
