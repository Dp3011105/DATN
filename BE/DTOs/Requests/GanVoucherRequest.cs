namespace BE.DTOs.Requests
{
    public class GanVoucherRequest
    {
        public List<int> ID_Khach_Hang { get; set; } = new List<int>();
        public List<int> ID_Voucher { get; set; } = new List<int>();

        public int SoLuong { get; set; }
        public string GhiChu { get; set; }
    }
}
