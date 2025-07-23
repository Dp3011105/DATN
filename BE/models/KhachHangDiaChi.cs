using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class KhachHangDiaChi
    {
        [Key, Column(Order = 0)]
        public int ID_Dia_Chi { get; set; }

        [Key, Column(Order = 1)]
        public int KhachHang_ID { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        public bool? Trang_Thai { get; set; }

        [ForeignKey("ID_Dia_Chi")]
        public virtual DiaChi DiaChi { get; set; }

        [ForeignKey("KhachHang_ID")]
        public virtual KhachHang KhachHang { get; set; }
    }
}
