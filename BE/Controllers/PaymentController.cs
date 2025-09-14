using BE.DTOs;
using BE.Repository;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IHoaDonRepository _hoaDonRepository;

        public PaymentController(IVnPayService vnPayService, IHoaDonRepository hoaDonRepository)
        {
            _vnPayService = vnPayService;
            _hoaDonRepository = hoaDonRepository;
        }

        // API tạo link thanh toán
        [HttpPost("create")] 
        public IActionResult CreatePaymentUrl([FromBody] PaymentInformationModel model)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(new { paymentUrl });
        }



        // đoạn này sẽ nhận trạng thái thanh toán , vnpay sẽ trả về VnPayResponseCode == "00" thì thanh toán thành công thực hiện đổi trạng than hóa đơn
        // còn ngoài 00 thì thanh toán thất bại hoặc hủy giao dịch = hủy đơn hàng = lý do hủy thanh toán vnpay
        // những cái hóa đơn mà  lý do hủy thanh toán vnpay chắ tui sẽ không hiển thị ở đơn hàng của khách hàng nữa

        //[HttpGet("PaymentCallbackVnpay")] 
        //public async Task<IActionResult> PaymentCallbackVnpay()
        //{
        //    var response = _vnPayService.PaymentExecute(Request.Query);

        //    var hoaDon = await _hoaDonRepository.GetByMaHoaDonAsync(response.OrderId);
        //    if (hoaDon != null)
        //    {
        //        if (response.Success && response.VnPayResponseCode == "00")
        //        {
        //            // ✅ Thanh toán thành công
        //            hoaDon.Trang_Thai = "Da_Xac_Nhan";
        //            await _hoaDonRepository.UpdateAsync(hoaDon);

        //            return Redirect("https://localhost:7081/CheckOutTk/vnpaydone");
        //        }
        //        else
        //        {
        //            // ❌ Thanh toán thất bại hoặc hủy
        //            hoaDon.Trang_Thai = "Huy_Don";
        //            hoaDon.LyDoHuyDon = "Hủy Thanh Toán VNPAY";
        //            await _hoaDonRepository.UpdateAsync(hoaDon);

        //            return Redirect("https://localhost:7081/HomeKhachHang");
        //        }
        //    }

        //    // Không tìm thấy hóa đơn
        //    return Redirect("https://localhost:7081/HomeKhachHang");
        //}


        [HttpGet("PaymentCallbackVnpay")]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            var hoaDon = await _hoaDonRepository.GetByMaHoaDonAsync(response.OrderId);
            if (hoaDon != null)
            {
                if (response.Success && response.VnPayResponseCode == "00")
                {
                    // ✅ Thanh toán thành công: Gọi UpdateAsync với code "00" để set "Da_Xac_Nhan" và trừ kho
                    await _hoaDonRepository.UpdateAsync(hoaDon, "00");

                    return Redirect("https://localhost:7081/CheckOutTk/vnpaydone");
                }
                else
                {
                    // ❌ Thanh toán thất bại hoặc hủy (giữ nguyên)
                    hoaDon.Trang_Thai = "Huy_Don";
                    hoaDon.LyDoHuyDon = "Hủy Thanh Toán VNPAY";
                    await _hoaDonRepository.UpdateAsync(hoaDon);  // Gọi UpdateAsync mà không truyền code để trigger phần fail

                    return Redirect("https://localhost:7081/HomeKhachHang");
                }
            }

            // Không tìm thấy hóa đơn (giữ nguyên)
            return Redirect("https://localhost:7081/HomeKhachHang");
        }







    }



}

