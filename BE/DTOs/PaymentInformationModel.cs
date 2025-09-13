namespace BE.DTOs
{
    public class PaymentInformationModel
    {
        public string OrderId { get; set; }           // Mã đơn hàng
        public string OrderType { get; set; }      // Loại giao dịch
        public decimal Amount { get; set; }        // Số tiền
        public string OrderDescription { get; set; } // Mô tả
        public string Name { get; set; }           // Tên người thanh toán
    }
}
