namespace FE.Models
{
    public class CheckoutTkViewModel
    {
        public List<HinhThucThanhToanCheckOutTK> PaymentMethods { get; set; }
        public List<DiaChiCheckOutTK> Addresses { get; set; }
        public List<VoucherCheckOutTK> Vouchers { get; set; }
        public List<ChiTietGioHangCheckOutTK> CartItems { get; set; } // Thêm thuộc tính CartItems
        public DiaChiCheckOutTK NewAddress { get; set; }
    }
}
