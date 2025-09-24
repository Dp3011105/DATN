using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using BE.models; // Namespace của models
using BE.Data;

namespace BE.Services
{
    public class ExpirationCheckService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpirationCheckService> _logger;
        private readonly TimeZoneInfo _vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        public ExpirationCheckService(IServiceProvider serviceProvider, ILogger<ExpirationCheckService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var nowVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _vnTimeZone);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                        // Kiểm tra Voucher
                        var vouchers = await dbContext.Voucher
                            .Where(v => v.Trang_Thai == true && v.Ngay_Ket_Thuc < nowVn)
                            .ToListAsync(stoppingToken);

                        foreach (var voucher in vouchers)
                        {
                            voucher.Trang_Thai = false;
                        }

                        // Kiểm tra KhuyenMai
                        var khuyenMais = await dbContext.KhuyenMai
                            .Where(k => k.Trang_Thai == true && k.Ngay_Ket_Thuc < nowVn)
                            .ToListAsync(stoppingToken);

                        foreach (var km in khuyenMais)
                        {
                            km.Trang_Thai = false;
                        }

                        await dbContext.SaveChangesAsync(stoppingToken);
                    }

                    _logger.LogInformation("Đã kiểm tra và cập nhật trạng thái hết hạn.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi kiểm tra hết hạn.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

    }
}