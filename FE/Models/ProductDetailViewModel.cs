namespace FE.Models
{
    public class ProductDetailViewModel
    {
        public List<DoNgot> DoNgots { get; set; } = new List<DoNgot>();
        public List<Size> Sizes { get; set; } = new List<Size>();
        public List<Topping> Toppings { get; set; } = new List<Topping>();
        public List<LuongDa> LuongDas { get; set; } = new List<LuongDa>();
    }

   
}
