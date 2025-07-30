namespace FE.Models
{
    public class UpdateProductViewModel
    {
        public int ID_San_Pham { get; set; }
        public string TenSanPham { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string MoTa { get; set; }
        public IFormFile Image { get; set; }
        public string CurrentImagePath { get; set; } // Lưu đường dẫn hình ảnh hiện tại

        public List<int> SelectedSizes { get; set; } = new List<int>();
        public List<int> SelectedToppings { get; set; } = new List<int>();
        public List<int> SelectedLuongDas { get; set; } = new List<int>();
        public List<int> SelectedDoNgots { get; set; } = new List<int>();
    }
}
