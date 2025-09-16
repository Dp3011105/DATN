using FE.Models;
using System.Text;
using System.Text.Json;
using FE.Models;
using FE.Service.IService;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace FE.Service
{
    public class CheckoutCKService
    {
        private readonly HttpClient _httpClient;

        public CheckoutCKService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Size>> GetSizesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7169/api/Size");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Size>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi API Size: {ex.Message}");
                throw;
            }
        }

        public async Task<List<HinhThucThanhToanCheckOutTK>> GetPaymentMethodsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7169/api/BanHangTK/hinhthucthanhtoan");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<HinhThucThanhToanCheckOutTK>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi API phương thức thanh toán: {ex.Message}");
                throw;
            }
        }

        public async Task<List<VoucherCheckOutTK>> GetVouchersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7169/api/BanHangCK/vouchers");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<VoucherCheckOutTK>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi API voucher: {ex.Message}");
                throw;
            }
        }

      
    }

  
}
