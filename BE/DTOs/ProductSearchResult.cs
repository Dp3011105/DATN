namespace BE.DTOs
{
    public class ProductSearchResult
    {
        public string Ten_San_Pham { get; set; } = string.Empty;
        public int So_Luong { get; set; }
        public List<string> Sizes { get; set; } = new List<string>();
        public List<string> Toppings { get; set; } = new List<string>();
        public bool Co_Khuyen_Mai { get; set; }
    }
}
