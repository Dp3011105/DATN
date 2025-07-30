using FE.Models;

namespace FE.Models
{
     public class SanPham
    {
        public int ID_San_Pham { get; set; }
        public string Ten_San_Pham { get; set; }
        public decimal Gia { get; set; }
        public int So_Luong { get; set; }
        public string Hinh_Anh { get; set; }
        public string Mo_Ta { get; set; }
        public bool Trang_Thai { get; set; }

        public List<Size> Sizes { get; set; }
        public List<LuongDa> LuongDas { get; set; }
        public List<DoNgot> DoNgots { get; set; }
        public List<Topping> Toppings { get; set; }
    }
}
