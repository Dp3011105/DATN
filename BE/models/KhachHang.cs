using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class KhachHang
    {
        [Key]
        public int ID_Khach_Hang { get; set; }

        [StringLength(100)]
        public string Ho_Ten { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public bool? GioiTinh { get; set; }

        [StringLength(20)]
        public string So_Dien_Thoai { get; set; }

        public bool? Trang_Thai { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        public virtual List<Gio_Hang> Gio_Hangs { get; set; }
        public virtual ICollection<KhachHangDiaChi> KhachHangDiaChis { get; set; } = new List<KhachHangDiaChi>();
        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
        public virtual ICollection<KhachHangVoucher> KhachHangVouchers { get; set; } = new List<KhachHangVoucher>();
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
        public virtual ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>(); // Thêm
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>(); // Thêm
    }
}
