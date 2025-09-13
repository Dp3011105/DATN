using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BE.models
{
    public class HoaDonChiTiet
    {
        [Key]
        public int ID_HoaDon_ChiTiet { get; set; }

        // ==== FK nhận từ client ====
        [Required] public int ID_Hoa_Don { get; set; }      // FK -> HoaDon
        [Required] public int ID_San_Pham { get; set; }     // FK -> SanPham
        public int? ID_Size { get; set; }                   // FK -> Size (optional)
        public int? ID_SanPham_DoNgot { get; set; }         // FK -> DoNgot (optional)
        public int? ID_LuongDa { get; set; }                // FK -> LuongDa (optional)

        // ==== Số liệu ====
        [Required] public string Ma_HoaDon_ChiTiet { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia_Them_Size { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia_San_Pham { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tong_Tien { get; set; }

        [Required] public int So_Luong { get; set; }
        [StringLength(255)] public string? Ghi_Chu { get; set; }

        [Required] public DateTime Ngay_Tao { get; set; } = DateTime.Now;

        // ==== Navigation: tránh bind JSON ====
        [ForeignKey(nameof(ID_Hoa_Don))]
        [JsonIgnore, ValidateNever] public virtual HoaDon? HoaDon { get; set; }

        [ForeignKey(nameof(ID_San_Pham))]
        [JsonIgnore, ValidateNever] public virtual SanPham? SanPham { get; set; }

        [ForeignKey(nameof(ID_Size))]
        [JsonIgnore, ValidateNever] public virtual Size? Size { get; set; }

        // Map chuẩn từ ID_SanPham_DoNgot -> DoNgot
        [ForeignKey(nameof(ID_SanPham_DoNgot))]
        [JsonIgnore, ValidateNever] public virtual DoNgot? DoNgot { get; set; }

        [ForeignKey(nameof(ID_LuongDa))]
        [JsonIgnore, ValidateNever] public virtual LuongDa? LuongDa { get; set; }

        [ ValidateNever]
        public virtual List<HoaDonChiTietTopping> HoaDonChiTietToppings { get; set; } = new();

        [JsonIgnore, ValidateNever]
        public virtual List<HoaDonChiTietThue> HoaDonChiTietThues { get; set; } = new();

    }
}
