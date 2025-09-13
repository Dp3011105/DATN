namespace FE.Models
{
    public class ProductViewModel
    {
        public List<SanPham> AllProducts { get; set; } = new List<SanPham>();
        public List<SanPham> MostPurchasedProducts { get; set; } = new List<SanPham>();
    }
}
