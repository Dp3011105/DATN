using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        public string TenSanPham { get; set; }
        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(1000, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 1.000")]
        public decimal Gia { get; set; }
        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 1")]
        public int SoLuong { get; set; }
        public string MoTa { get; set; }
        [Required(ErrorMessage = "Hình ảnh là bắt buộc")]
        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "Phải chọn ít nhất một kích thước")]
        public List<int> SelectedSizes { get; set; } = new List<int>();
        public List<int> SelectedToppings { get; set; } = new List<int>();
        [Required(ErrorMessage = "Phải chọn ít nhất một lượng đá")]
        public List<int> SelectedLuongDas { get; set; } = new List<int>();
        [Required(ErrorMessage = "Phải chọn ít nhất một độ ngọt")]
        public List<int> SelectedDoNgots { get; set; } = new List<int>();
    }
}
