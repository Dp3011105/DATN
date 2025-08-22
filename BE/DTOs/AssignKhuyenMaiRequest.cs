namespace BE.DTOs
{
    public class AssignKhuyenMaiRequest
    {
        public int ID_Khuyen_Mai { get; set; }
        public List<int> ID_San_Phams { get; set; }
        public decimal Phan_Tram_Giam { get; set; } 
    }
}
