using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class VoucherViewModel
    {
        public int ID_Voucher { get; set; }

        [Required(ErrorMessage = "Mã voucher không được để trống")]
        [StringLength(50, ErrorMessage = "Mã voucher không được vượt quá 50 ký tự")]
        public string Ma_Voucher { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Tên voucher không được vượt quá 100 ký tự")]
        public string? Ten { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int? So_Luong { get; set; }

        // SỬA: Validation cho phần trăm giảm (1-100%)
        [Range(1, 100, ErrorMessage = "Phần trăm giảm phải từ 1% đến 100%")]
        [Display(Name = "Phần trăm giảm (%)")]
        public decimal? Gia_Tri_Giam { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Yêu cầu tối thiểu phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Yêu cầu tối thiểu (VNĐ)")]
        public decimal? So_Tien_Dat_Yeu_Cau { get; set; }

        public DateTime? Ngay_Bat_Dau { get; set; }

        public DateTime? Ngay_Ket_Thuc { get; set; }

        public bool? Trang_Thai { get; set; }

        // THÊM: Kiểm tra hết hạn
        public bool IsExpired => Ngay_Ket_Thuc.HasValue && Ngay_Ket_Thuc.Value < DateTime.Now;

        // SỬA: Hiển thị trạng thái với ưu tiên (Hết hạn > Hoạt động/Ngừng)
        public string TrangThaiText
        {
            get
            {
                if (IsExpired)
                    return "Hết hạn";

                return Trang_Thai == true ? "Hoạt động" : "Ngừng hoạt động";
            }
        }

        public string TrangThaiClass
        {
            get
            {
                if (IsExpired)
                    return "badge bg-warning";

                return Trang_Thai == true ? "badge bg-success" : "badge bg-danger";
            }
        }

        // THÊM: Thứ tự sắp xếp (1=Hoạt động, 2=Ngừng, 3=Hết hạn)
        public int TrangThaiOrder
        {
            get
            {
                if (IsExpired)
                    return 3; // Hết hạn

                return Trang_Thai == true ? 1 : 2; // Hoạt động : Ngừng hoạt động
            }
        }

        // THÊM: Hiển thị phần trăm giảm
        public string GiaTriGiamText => Gia_Tri_Giam.HasValue ? $"{Gia_Tri_Giam}%" : "0%";

        // THÊM: Hiển thị yêu cầu tối thiểu
        public string SoTienDatYeuCauText => So_Tien_Dat_Yeu_Cau.HasValue ?
            $"{So_Tien_Dat_Yeu_Cau:N0} VNĐ" : "Không yêu cầu";
    }
}