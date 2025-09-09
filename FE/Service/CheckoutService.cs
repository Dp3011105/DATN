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
    public class CheckoutService : ICheckoutService
    {
        private readonly HttpClient _httpClient;

        public CheckoutService(HttpClient httpClient)
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
            var response = await _httpClient.GetAsync("https://localhost:7169/api/BanHangTK/hinhthucthanhtoan");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<HinhThucThanhToanCheckOutTK>>(content);
        }

        public async Task<List<DiaChiCheckOutTK>> GetCustomerAddressesAsync(string customerId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7169/api/BanHangTK/khachhang/{customerId}/diachi");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DiaChiCheckOutTK>>(content);
        }

        public async Task<List<VoucherCheckOutTK>> GetVouchersAsync(string customerId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7169/api/BanHangTK/vouchers/{customerId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<VoucherCheckOutTK>>(content);
        }

        public async Task<List<ChiTietGioHangCheckOutTK>> GetCartItemsByIdsAsync(string customerId, List<int> selectedIds)
        {
            try
            {
                Console.WriteLine($"Gọi API giỏ hàng: https://localhost:7169/api/Gio_Hang/{customerId}");
                var response = await _httpClient.GetAsync($"https://localhost:7169/api/Gio_Hang/{customerId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Lỗi API: StatusCode={response.StatusCode}, Reason={response.ReasonPhrase}");
                    return new List<ChiTietGioHangCheckOutTK>();
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Dữ liệu API trả về: {json}");

                List<ChiTietGioHangCheckOutTK> cartItems = null;

                // Thử deserialize theo cấu trúc CartResponse
                try
                {
                    var cartData = JsonSerializer.Deserialize<CartResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    cartItems = cartData?.Chi_Tiet_Gio_Hang;
                    Console.WriteLine($"Deserialize thành công với CartResponse, số mục: {cartItems?.Count ?? 0}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Lỗi deserialize với CartResponse: {ex.Message}");
                }

                // Nếu thất bại, thử danh sách trực tiếp
                if (cartItems == null)
                {
                    try
                    {
                        cartItems = JsonSerializer.Deserialize<List<ChiTietGioHangCheckOutTK>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        Console.WriteLine($"Deserialize thành công với danh sách trực tiếp, số mục: {cartItems?.Count ?? 0}");
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Lỗi deserialize danh sách trực tiếp: {ex.Message}, Input: {json}");
                        return new List<ChiTietGioHangCheckOutTK>();
                    }
                }

                if (cartItems == null || !cartItems.Any())
                {
                    Console.WriteLine("Dữ liệu giỏ hàng rỗng hoặc không có mục nào.");
                    return new List<ChiTietGioHangCheckOutTK>();
                }

                // Lọc chỉ các mục có ID_GioHang_ChiTiet khớp với selectedIds
                var filteredItems = cartItems
                    .Where(item => selectedIds.Contains(item.ID_GioHang_ChiTiet))
                    .ToList();

                Console.WriteLine($"Số mục được chọn từ cookie sau lọc: {filteredItems.Count}");
                foreach (var item in filteredItems)
                {
                    Console.WriteLine($"Mục từ API: ID_GioHang_ChiTiet={item.ID_GioHang_ChiTiet}, Ten_San_Pham={item.Ten_San_Pham}, So_Luong={item.So_Luong}, Toppings={JsonSerializer.Serialize(item.Toppings ?? new List<ToppingCheckOutTK>())}");
                }

                return filteredItems;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Lỗi khi gọi API giỏ hàng: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định trong GetCartItemsByIdsAsync: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> AddAddressAsync(string customerId, DiaChiCheckOutTK address)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(address);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://localhost:7169/api/BanHangTK/khachhang/{customerId}/diachi", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi khi thêm địa chỉ: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi API thêm địa chỉ: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAddressAsync(string customerId, DiaChiCheckOutTK address)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(address);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"https://localhost:7169/api/BanHangTK/khachhang/{customerId}/diachi/{address.ID_Dia_Chi}", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi khi cập nhật địa chỉ: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi API cập nhật địa chỉ: {ex.Message}");
                return false;
            }
        }

        public async Task<object> ProcessCheckoutAsync(CheckoutModel model)
        {
            try
            {
                // Validate input
                if (model == null || model.iD_Khach_Hang == 0 || model.hoaDonChiTiets == null || !model.hoaDonChiTiets.Any())
                {
                    Console.WriteLine("Dữ liệu checkout không hợp lệ");
                    return new { Success = false, Message = "Dữ liệu checkout không hợp lệ" };
                }

                // Fetch sizes
                var sizes = await GetSizesAsync();
                if (sizes == null || !sizes.Any())
                {
                    Console.WriteLine("Không thể lấy danh sách kích thước");
                    return new { Success = false, Message = "Không thể lấy danh sách kích thước" };
                }

                // Validate cart items
                var cartItems = await GetCartItemsByIdsAsync(model.iD_Khach_Hang.ToString(), model.hoaDonChiTiets.Select(h => h.ma_Hoa_Don_ChiTiet.Replace("HDC", "")).Select(int.Parse).ToList());
                if (cartItems == null || !cartItems.Any())
                {
                    Console.WriteLine("Không tìm thấy mục giỏ hàng hợp lệ");
                    return new { Success = false, Message = "Không tìm thấy mục giỏ hàng hợp lệ" };
                }

                // Calculate total and validate
                decimal calculatedTotal = 0;
                foreach (var item in model.hoaDonChiTiets)
                {
                    var cartItem = cartItems.FirstOrDefault(c => c.ID_GioHang_ChiTiet == int.Parse(item.ma_Hoa_Don_ChiTiet.Replace("HDC", "")));
                    if (cartItem == null)
                    {
                        Console.WriteLine($"Mục giỏ hàng {item.ma_Hoa_Don_ChiTiet} không tồn tại");
                        return new { Success = false, Message = $"Mục giỏ hàng {item.ma_Hoa_Don_ChiTiet} không tồn tại" };
                    }

                    var sizePrice = sizes.FirstOrDefault(s => s.ID_Size == item.iD_Size)?.Gia ?? 0;
                    var toppingTotal = item.hoaDonChiTietToppings?.Sum(t => t.gia_Topping) ?? 0;
                    var itemTotal = (item.gia_San_Pham + sizePrice) * item.so_Luong;
                    calculatedTotal += itemTotal;
                }

                // Apply voucher if present
                if (model.iD_Voucher != 0)
                {
                    var vouchers = await GetVouchersAsync(model.iD_Khach_Hang.ToString());
                    var voucher = vouchers.FirstOrDefault(v => v.ID_Voucher == model.iD_Voucher);
                    if (voucher == null)
                    {
                        Console.WriteLine($"Voucher {model.iD_Voucher} không hợp lệ");
                        return new { Success = false, Message = "Voucher không hợp lệ" };
                    }
                    if (calculatedTotal < voucher.So_Tien_Dat_Yeu_Cau)
                    {
                        Console.WriteLine($"Đơn hàng không đạt yêu cầu tối thiểu cho voucher {voucher.ID_Voucher}");
                        return new { Success = false, Message = "Đơn hàng không đạt yêu cầu tối thiểu cho voucher" };
                    }
                    calculatedTotal *= (1 - (decimal)voucher.Gia_Tri_Giam / 100);
                }

                // Validate total
                if (Math.Abs(calculatedTotal - model.tong_Tien) > 0.01m)
                {
                    Console.WriteLine($"Tổng tiền không khớp: Tính toán={calculatedTotal}, Gửi lên={model.tong_Tien}");
                    return new { Success = false, Message = "Tổng tiền không khớp" };
                }

                // Prepare checkout data for API
                var checkoutData = new
                {
                    ID_Khach_Hang = model.iD_Khach_Hang,
                    ID_Hinh_Thuc_Thanh_Toan = model.iD_Hinh_Thuc_Thanh_Toan,
                    ID_Dia_Chi = model.iD_Dia_Chi,
                    ID_Voucher = model.iD_Voucher,
                    Tong_Tien = model.tong_Tien,
                    Ghi_Chu = model.ghi_Chu,
                    Ma_Hoa_Don = model.ma_Hoa_Don,
                    HoaDonChiTiets = model.hoaDonChiTiets.Select(h => new
                    {
                        ID_San_Pham = h.iD_San_Pham,
                        ID_Size = h.iD_Size,
                        ID_SanPham_DoNgot = h.iD_SanPham_DoNgot,
                        ID_LuongDa = h.iD_LuongDa,
                        Ma_Hoa_Don_ChiTiet = h.ma_Hoa_Don_ChiTiet,
                        Gia_Them_Size = sizes.FirstOrDefault(s => s.ID_Size == h.iD_Size)?.Gia ?? 0,
                        Gia_San_Pham = h.gia_San_Pham,
                        Tong_Tien = h.tong_Tien,
                        So_Luong = h.so_Luong,
                        Ghi_Chu = h.ghi_Chu,
                        HoaDonChiTietToppings = h.hoaDonChiTietToppings.Select(t => new
                        {
                            ID_Topping = t.iD_Topping,
                            So_Luong = t.so_Luong,
                            Gia_Topping = t.gia_Topping
                        }).ToList()
                    }).ToList()
                };

                // Call API to save order
                var jsonContent = JsonSerializer.Serialize(checkoutData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7169/api/BanHangTK/checkout", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<object>(responseData);
                    Console.WriteLine($"Checkout thành công: {responseData}");
                    return new { Success = true, MaHoaDon = model.ma_Hoa_Don, Data = result };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi khi xử lý checkout: {errorContent}");
                    return new { Success = false, Message = $"Lỗi khi xử lý checkout: {errorContent}" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xử lý checkout: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new { Success = false, Message = $"Lỗi khi xử lý checkout: {ex.Message}" };
            }
        }
    }
}