using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class Message
    {
        [Key]
        public int ID_Message { get; set; }

        [Required]
        public int ID_Chat_Session { get; set; }

        public int? ID_Khach_Hang { get; set; } // Người gửi là khách hàng (null nếu là nhân viên)
        public int? ID_Nhan_Vien { get; set; } // Người gửi là nhân viên (null nếu là khách hàng)

        [Required]
        public string Noi_Dung { get; set; } // Nội dung tin nhắn

        public DateTime Thoi_Gian_Gui { get; set; } // Thời gian gửi tin nhắn

        public bool Trang_Thai { get; set; } // Trạng thái tin nhắn (true: đã gửi, false: đã xóa)

        [ForeignKey("ID_Chat_Session")]
        public virtual ChatSession ChatSession { get; set; }

        [ForeignKey("ID_Khach_Hang")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("ID_Nhan_Vien")]
        public virtual NhanVien NhanVien { get; set; }
    }
}
