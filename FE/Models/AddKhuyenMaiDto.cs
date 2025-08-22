namespace FE.Models
{
    public class AddKhuyenMaiDto
    {
        public int ID_Khuyen_Mai { get; set; }
        public List<int> ID_San_Phams { get; set; } = new List<int>();
        public int Phan_Tram_Giam { get; set; }
    }
}
