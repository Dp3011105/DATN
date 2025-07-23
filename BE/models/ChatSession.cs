using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class ChatSession
    {
        [Key]
        public int ID_Chat_Session { get; set; }

        [Required]
        public int ID_Khach_Hang { get; set; }

        [StringLength(255)]
        public string Tieu_De { get; set; } // Tiêu đề phiên chat (tùy chọn, ví dụ: "Hỗ trợ đơn hàng #123")

        public bool Trang_Thai { get; set; } // Trạng thái phiên chat (true: đang mở, false: đã đóng)

        [ForeignKey("ID_Khach_Hang")]
        public virtual KhachHang KhachHang { get; set; }

        public virtual ICollection<ChatSessionNhanVien> ChatSessionNhanViens { get; set; } = new List<ChatSessionNhanVien>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
