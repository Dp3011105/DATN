using FE.Models;
using FE.Service.IService;
using System.Text;
using System.Text.Json;

namespace FE.Service
{
    public class VoucherService : IVoucherService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public VoucherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        // S?A: S?p x?p theo th? t? ?u tiên
        public async Task<IEnumerable<VoucherViewModel>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/voucher");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vouchers = JsonSerializer.Deserialize<IEnumerable<VoucherViewModel>>(json, _jsonOptions)
                               ?? new List<VoucherViewModel>();

                // S?p x?p theo th? t?: Ho?t ??ng ? Ng?ng ho?t ??ng ? H?t h?n
                return vouchers.OrderBy(v => v.TrangThaiOrder).ThenByDescending(v => v.ID_Voucher);
            }

            return new List<VoucherViewModel>();
        }

        // THÊM: L?y vouchers ho?t ??ng
        public async Task<IEnumerable<VoucherViewModel>> GetActiveVouchersAsync()
        {
            var response = await _httpClient.GetAsync("api/voucher/active");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<VoucherViewModel>>(json, _jsonOptions)
                       ?? new List<VoucherViewModel>();
            }

            return new List<VoucherViewModel>();
        }

        public async Task<VoucherViewModel?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/voucher/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<VoucherViewModel>(json, _jsonOptions);
            }

            return null;
        }

        public async Task<VoucherViewModel?> GetByCodeAsync(string code)
        {
            var response = await _httpClient.GetAsync($"api/voucher/code/{code}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<VoucherViewModel>(json, _jsonOptions);
            }

            return null;
        }

        // THÊM: Validate voucher
        public async Task<object> ValidateVoucherAsync(string code, decimal orderAmount)
        {
            var request = new { Code = code, OrderAmount = orderAmount };
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/voucher/validate", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(responseJson, _jsonOptions)!;
            }

            throw new Exception($"L?i ki?m tra voucher: {response.StatusCode}");
        }

        public async Task<VoucherViewModel> CreateAsync(VoucherViewModel voucher)
        {
            var json = JsonSerializer.Serialize(voucher, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/voucher", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<VoucherViewModel>(responseJson, _jsonOptions)!;
            }

            // S?A: Hi?n th? l?i chi ti?t h?n
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"L?i t?o voucher: {errorContent}");
        }

        public async Task<VoucherViewModel> UpdateAsync(VoucherViewModel voucher)
        {
            var json = JsonSerializer.Serialize(voucher, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/voucher/{voucher.ID_Voucher}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<VoucherViewModel>(responseJson, _jsonOptions)!;
            }

            // S?A: Hi?n th? l?i chi ti?t h?n
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"L?i c?p nh?t voucher: {errorContent}");
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var response = await _httpClient.PostAsync($"api/voucher/deactivate/{id}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckCodeExistsAsync(string code, int? excludeId = null)
        {
            var url = $"api/voucher/check-code/{code}";
            if (excludeId.HasValue)
            {
                url += $"?excludeId={excludeId}";
            }

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(json, _jsonOptions);
            }

            return false;
        }
        public async Task<bool> BulkActivateAsync(List<int> voucherIds)
        {
            try
            {
                // Sử dụng vòng lặp để activate từng voucher
                foreach (int id in voucherIds)
                {
                    // Gọi API để activate voucher
                    var response = await _httpClient.PostAsync($"api/voucher/activate/{id}", null);
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> BulkDeactivateAsync(List<int> voucherIds)
        {
            try
            {
                // Sử dụng vòng lặp để deactivate từng voucher  
                foreach (int id in voucherIds)
                {
                    // Sử dụng lại endpoint deactivate hiện có
                    var response = await _httpClient.PostAsync($"api/voucher/deactivate/{id}", null);
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}