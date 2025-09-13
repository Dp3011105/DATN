using BE.DTOs;
using BE.Repository.IRepository;

namespace BE.Repository
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);

            // ✅ Nhúng OrderId vào OrderInfo để khi callback dễ log
            pay.AddRequestData("vnp_OrderInfo",
                $"OrderId={model.OrderId}; {model.Name} - {model.OrderDescription}");

            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:ReturnUrl"]);

            // ✅ Mã đơn hàng gốc (string) đưa vào vnp_TxnRef
            pay.AddRequestData("vnp_TxnRef", model.OrderId);

            return pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    pay.AddResponseData(key, value);
                }
            }

            var response = new PaymentResponseModel
            {
                OrderId = pay.GetResponseData("vnp_TxnRef"),          // Mã đơn hàng (string)
                TransactionId = pay.GetResponseData("vnp_TransactionNo"),
                OrderDescription = pay.GetResponseData("vnp_OrderInfo"),
                PaymentMethod = "VnPay",
                PaymentId = pay.GetResponseData("vnp_TransactionNo"),
                Success = pay.ValidateSignature(collections["vnp_SecureHash"], _configuration["Vnpay:HashSecret"]),
                Token = collections["vnp_SecureHash"],
                VnPayResponseCode = pay.GetResponseData("vnp_ResponseCode")
            };

            return response;
        }
    }
}
