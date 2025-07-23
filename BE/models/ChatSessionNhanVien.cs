using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class ChatSessionNhanVien
    {
        [Key, Column(Order = 0)]
        public int ID_Chat_Session { get; set; }

        [Key, Column(Order = 1)]
        public int ID_Nhan_Vien { get; set; }

        [ForeignKey("ID_Chat_Session")]
        public virtual ChatSession ChatSession { get; set; }

        [ForeignKey("ID_Nhan_Vien")]
        public virtual NhanVien NhanVien { get; set; }
    }
}
