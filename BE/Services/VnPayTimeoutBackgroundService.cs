using BE.Data;
using Microsoft.EntityFrameworkCore;

namespace BE.Services
{
    public class VnPayTimeoutBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VnPayTimeoutBackgroundService> _logger;

        //  Thời gian hết hạn đơn VNPAY 15 phút
        private readonly TimeSpan _expireTime = TimeSpan.FromMinutes(15);
        //private readonly TimeSpan _expireTime = TimeSpan.FromSeconds(10); // test 

        // Khoảng delay giữa mỗi lần check 1 phút 
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);
        //private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(5);

        public VnPayTimeoutBackgroundService(IServiceProvider serviceProvider,
                                             ILogger<VnPayTimeoutBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 VnPayTimeoutBackgroundService đã khởi động.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                        // ✅ Lấy thời gian hiện tại
                        var now = DateTime.Now;

                        // ✅ Mốc quá hạn
                        var threshold = now - _expireTime;

                        // ✅ Lấy các hóa đơn VNPAY chờ quá hạn
                        var expiredOrders = await db.Hoa_Don
                            .Where(h => h.Trang_Thai == "Chua_Thanh_Toan"      && h.Ngay_Tao < threshold)
                            .ToListAsync(stoppingToken);

                        if (expiredOrders.Any())
                        {
                            foreach (var order in expiredOrders)
                            {
                                order.Trang_Thai = "Huy_Don";
                                order.LyDoHuyDon = "Quá hạn thanh toán VNPAY";
                            }

                            await db.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation($"✅ Đã hủy {expiredOrders.Count} hóa đơn VNPAY quá hạn (thời điểm: {now}).");
                        }
                        else
                        {
                            _logger.LogInformation($"⏳ Không có hóa đơn VNPAY nào quá hạn (thời điểm: {now}).");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Lỗi khi xử lý hóa đơn VNPAY quá hạn.");
                }

                // ⏱ Chờ 1 phút rồi chạy tiếp
                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("🛑 VnPayTimeoutBackgroundService đã dừng.");
        }
    }
}
