namespace BE.DTOs
{
    public class PaymentResponseModel
    {
        public string OrderId { get; set; }            // Mã đơn hàng
        public string TransactionId { get; set; }      // Mã giao dịch VNPAY
        public string OrderDescription { get; set; }   // Nội dung đơn hàng
        public string PaymentMethod { get; set; }      // Hình thức thanh toán
        public string PaymentId { get; set; }          // PaymentId
        public bool Success { get; set; }              // Kết quả thanh toán
        public string Token { get; set; }              // Chuỗi hash
        public string VnPayResponseCode { get; set; }  // Mã phản hồi từ VNPAY
    }
}
