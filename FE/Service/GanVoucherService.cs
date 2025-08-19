using BE.models;
using FE.Models;
using Newtonsoft.Json;
using System.Text;

namespace FE.Service
{
    public class GanVoucherService : IGanVoucherService
    {
        private readonly HttpClient _http;

        public GanVoucherService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<KhachHang>> GetAllKhachHangAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/GanVoucher/khachhang-all");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<KhachHang>>(json) ?? new List<KhachHang>();
            }
            catch (Exception)
            {
                return new List<KhachHang>();
            }
        }

        // Lấy top 10 VIP
        public async Task<List<KhachHang>> GetTop10VipAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/GanVoucher/top10-vip");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<KhachHang>>(json) ?? new List<KhachHang>();
            }
            catch (Exception)
            {
                return new List<KhachHang>();
            }
        }

        public async Task<List<Voucher>> GetAllVouchersAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/GanVoucher/vouchers");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Voucher>>(json) ?? new List<Voucher>();
            }
            catch (Exception)
            {
                return new List<Voucher>();
            }
        }

        public async Task<GanVoucherResult> GanVoucherAsync(GanVoucherViewModel vm)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(vm);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync("api/GanVoucher/gan-voucher", content);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse response thành công
                    dynamic result = JsonConvert.DeserializeObject(responseJson);
                    return new GanVoucherResult
                    {
                        Success = result?.success ?? true,
                        Message = result?.message?.ToString() ?? "Gán voucher thành công!",
                        HasWarnings = result?.hasWarnings ?? false
                    };
                }
                else
                {
                    // Parse response lỗi
                    dynamic errorResult = JsonConvert.DeserializeObject(responseJson);
                    return new GanVoucherResult
                    {
                        Success = false,
                        Message = errorResult?.message?.ToString() ?? $"Lỗi HTTP {response.StatusCode}",
                        HasWarnings = false
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new GanVoucherResult
                {
                    Success = false,
                    Message = $"❌ Lỗi kết nối API: {httpEx.Message}",
                    HasWarnings = false
                };
            }
            catch (JsonException jsonEx)
            {
                return new GanVoucherResult
                {
                    Success = false,
                    Message = $"❌ Lỗi phân tích dữ liệu: {jsonEx.Message}",
                    HasWarnings = false
                };
            }
            catch (Exception ex)
            {
                return new GanVoucherResult
                {
                    Success = false,
                    Message = $"❌ Lỗi hệ thống: {ex.Message}",
                    HasWarnings = false
                };
            }
        }

        public async Task<List<KhachHangVoucher>> GetVouchersByKhachHangAsync(int khachHangId)
        {
            try
            {
                var response = await _http.GetAsync($"api/GanVoucher/vouchers-by-customer/{khachHangId}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<KhachHangVoucher>>(json) ?? new List<KhachHangVoucher>();
            }
            catch (Exception)
            {
                return new List<KhachHangVoucher>();
            }
        }
    }

    // Class để chứa kết quả gán voucher
    public class GanVoucherResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool HasWarnings { get; set; }
    }
}