using System.ComponentModel.DataAnnotations;

namespace BE.models
{
    public class Topping
    {
        [Key]
        public int ID_Topping { get; set; }

        [StringLength(100)]
        public string Ten { get; set; }

        public decimal? Gia { get; set; }

        public int? So_Luong { get; set; }

        public string Hinh_Anh { get; set; }

        [StringLength(255)]
        public string Ghi_Chu { get; set; }

        public bool? Trang_Thai { get; set; }

        // ✅ Thêm dòng này:
        public List<GioHangChiTiet_Topping> GioHangChiTiet_Toppings { get; set; } = new();

        public virtual ICollection<SanPhamTopping> SanPhamToppings { get; set; } = new List<SanPhamTopping>();
        public virtual ICollection<HoaDonChiTietTopping> HoaDonChiTietToppings { get; set; } = new List<HoaDonChiTietTopping>();
    }
}
