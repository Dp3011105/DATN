//using BE.Data;
//using Microsoft.EntityFrameworkCore;
//using System;

//namespace BE.Services
//{
//    public class VnPayTimeoutBackgroundService : BackgroundService
//    {
//        private readonly IServiceProvider _serviceProvider;
//        private readonly ILogger<VnPayTimeoutBackgroundService> _logger;

//        public VnPayTimeoutBackgroundService(IServiceProvider serviceProvider, ILogger<VnPayTimeoutBackgroundService> logger)
//        {
//            _serviceProvider = serviceProvider;
//            _logger = logger;
//        }

//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            var timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); //5 phút thực hiện check 1 lần
//            //var timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5)); ////5 giây thực hiện check 1 lần (test)
//            return Task.CompletedTask;
//        }

//        private async void DoWork(object? state)
//        {
//            using var scope = _serviceProvider.CreateScope();
//            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>(); 

//            try
//            {
//                // Lấy múi giờ Việt Nam (UTC+7)
//                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
//                var vietnamNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
//                var timeoutThreshold = vietnamNow.AddMinutes(-15);// sau 15 phút đặt hàng mà người dùng không thanh toán và không hủy đơn thì sẽ tự động hủy 
//                //var timeoutThreshold = vietnamNow.AddSeconds(-10);//10 giây mà không thanh toán hủy luôn (test)

//                // Truy vấn các hóa đơn VNPAY đang chờ quá 15 phút
//                var pendingInvoices = await context.Hoa_Don
//                    .Include(h => h.KhachHang)
//                    .ThenInclude(kh => kh.KhachHangVouchers)
//                    .Include(h => h.HoaDonVouchers)
//                    .ThenInclude(hdv => hdv.Voucher)
//                    .Where(h => h.Ghi_Chu == "VNPAY"
//                                && h.Trang_Thai == "Chua_Thanh_Toan"
//                                && h.Ngay_Tao < timeoutThreshold)
//                    .ToListAsync();

//                if (pendingInvoices.Any())
//                {
//                    foreach (var invoice in pendingInvoices)
//                    {
//                        using var tx = await context.Database.BeginTransactionAsync();

//                        // Cập nhật trạng thái và lý do hủy
//                        invoice.Trang_Thai = "Huy_Don";
//                        invoice.LyDoHuyDon = "Quá hạn thanh toán VNPAY (15 phút)";

//                        // Hoàn trả voucher nếu có
//                        if (invoice.HoaDonVouchers != null && invoice.HoaDonVouchers.Any())
//                        {
//                            var khachHangVoucher = invoice.KhachHang?.KhachHangVouchers
//                                .FirstOrDefault(khv => khv.ID_Voucher == invoice.HoaDonVouchers.First().ID_Voucher);

//                            if (khachHangVoucher != null)
//                            {
//                                khachHangVoucher.Trang_Thai = true; // Trả lại quyền sử dụng voucher
//                                context.KhachHang_Voucher.Update(khachHangVoucher);
//                            }
//                        }

//                        context.Hoa_Don.Update(invoice);
//                        await context.SaveChangesAsync();
//                        await tx.CommitAsync();
//                    }

//                    _logger.LogInformation($"Đã hủy {pendingInvoices.Count} hóa đơn VNPAY do quá hạn.");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi xảy ra khi kiểm tra thời hạn VNPAY.");
//            }
//        }

//        public override void Dispose()
//        {
//            base.Dispose();
//        }
//    }
//}
///// Vì khi thanhtoán VNPAY thành công thì trạng thái chưa thanh toán là đã trừ số lượng sản phẩm , nếu như người dùng chưa thanh toán mà qua hạn thì phải trả lại 
///số lượng sản phẩm và topping vào kho


using BE.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BE.Services
{
    public class VnPayTimeoutBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VnPayTimeoutBackgroundService> _logger;

        public VnPayTimeoutBackgroundService(IServiceProvider serviceProvider, ILogger<VnPayTimeoutBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
             //var timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5)); ////5 giây thực hiện check 1 lần (test)

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            try
            {
                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var vietnamNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
               var timeoutThreshold = vietnamNow.AddMinutes(-15);
               // var timeoutThreshold = vietnamNow.AddSeconds(-10);//10 giây mà không thanh toán hủy luôn (test)

                // ✅ Truy vấn các hóa đơn đang chờ quá 15 phút
                var pendingInvoices = await context.Hoa_Don
                    .Include(h => h.KhachHang)
                        .ThenInclude(kh => kh.KhachHangVouchers)
                    .Include(h => h.HoaDonVouchers)
                        .ThenInclude(hdv => hdv.Voucher)
                    .Include(h => h.HoaDonChiTiets)                  // chi tiết sản phẩm
                        .ThenInclude(hdct => hdct.SanPham)
                    .Include(h => h.HoaDonChiTiets)                  // chi tiết topping
                        .ThenInclude(hdct => hdct.HoaDonChiTietToppings)
                        .ThenInclude(hdctt => hdctt.Topping)
                    .Where(h => h.Ghi_Chu == "VNPAY"
                                && h.Trang_Thai == "Chua_Thanh_Toan"
                                && h.Ngay_Tao < timeoutThreshold)
                    .ToListAsync();

                if (pendingInvoices.Any())
                {
                    foreach (var invoice in pendingInvoices)
                    {
                        using var tx = await context.Database.BeginTransactionAsync();

                        // ✅ Cập nhật trạng thái hủy
                        invoice.Trang_Thai = "Huy_Don";
                        invoice.LyDoHuyDon = "Quá hạn thanh toán VNPAY (15 phút)";

                        // ✅ Hoàn trả số lượng sản phẩm
                        foreach (var hdct in invoice.HoaDonChiTiets)
                        {
                            if (hdct.SanPham != null)
                            {
                                hdct.SanPham.So_Luong += hdct.So_Luong;
                                context.San_Pham.Update(hdct.SanPham);
                            }

                            // ✅ Hoàn trả số lượng topping
                            foreach (var hdctt in hdct.HoaDonChiTietToppings)
                            {
                                if (hdctt.Topping != null)
                                {
                                    int soLuongTopping = hdctt.So_Luong ?? 0;
                                    hdctt.Topping.So_Luong += soLuongTopping;
                                    context.Topping.Update(hdctt.Topping);
                                }
                            }
                        }

                        // ✅ Hoàn trả voucher nếu có
                        if (invoice.HoaDonVouchers != null && invoice.HoaDonVouchers.Any())
                        {
                            var khachHangVoucher = invoice.KhachHang?.KhachHangVouchers
                                .FirstOrDefault(khv => khv.ID_Voucher == invoice.HoaDonVouchers.First().ID_Voucher);

                            if (khachHangVoucher != null)
                            {
                                khachHangVoucher.Trang_Thai = true;
                                context.KhachHang_Voucher.Update(khachHangVoucher);
                            }
                        }

                        context.Hoa_Don.Update(invoice);
                        await context.SaveChangesAsync();
                        await tx.CommitAsync();
                    }

                    _logger.LogInformation($"Đã hủy {pendingInvoices.Count} hóa đơn VNPAY quá hạn và hoàn kho sản phẩm + topping.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi kiểm tra thời hạn VNPAY.");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
