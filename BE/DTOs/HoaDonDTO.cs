public class HoaDonDTO
{
    public int ID_Hoa_Don { get; set; }
    public string Ma_Hoa_Don { get; set; } = "";
    public DateTime Ngay_Tao { get; set; }
    public decimal Tong_Tien { get; set; }
    public string Trang_Thai { get; set; } = "";

    // THÊM cho màn danh sách
    public string Loai_Hoa_Don { get; set; } = "";          // "TaiQuay" | "GiaoHang"
    public int? ID_Hinh_Thuc_Thanh_Toan { get; set; }
    public string? Ten_Hinh_Thuc_Thanh_Toan { get; set; }
    public string? Dia_Chi_Tu_Nhap { get; set; }
}
