using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class TaiKhoanVaiTro
    {
        [Key, Column(Order = 0)]
        public int ID_Vai_Tro { get; set; }

        [Key, Column(Order = 1)]
        public int ID_Tai_Khoan { get; set; }

        [ForeignKey("ID_Vai_Tro")]
        public virtual VaiTro VaiTro { get; set; }

        [ForeignKey("ID_Tai_Khoan")]
        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
