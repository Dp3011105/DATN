using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class NhanVien
    {
        [Key]
        public int ID_Nhan_Vien { get; set; }

        [Required]
        [StringLength(100)]
        public string Ho_Ten { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public bool? GioiTinh { get; set; }

        [StringLength(20)]
        public string So_Dien_Thoai { get; set; }

        [StringLength(255)]
        public string Dia_Chi { get; set; }

        public DateTime NamSinh { get; set; }

        [StringLength(20)]
        public string CCCD { get; set; }

        public bool? Trang_Thai { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        // New properties for images
        [StringLength(255)]
        public string? AnhNhanVien { get; set; } // Path or URL to employee image

        [StringLength(255)]
        public string? AnhCCCD { get; set; } // Path or URL to ID card image

        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
        public virtual ICollection<DiemDanh> DiemDanhs { get; set; } = new List<DiemDanh>();
        public virtual ICollection<ChatSessionNhanVien> ChatSessionNhanViens { get; set; } = new List<ChatSessionNhanVien>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}