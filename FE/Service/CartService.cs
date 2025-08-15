using FE.Models;
using FE.Service.IService;

namespace FE.Service
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7169/api/";

        public CartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GioHang> GetCartByUserIdAsync(int userId)// dùng cho giỏ hàng để get id khách hang 
        {
            return await _httpClient.GetFromJsonAsync<GioHang>($"{BaseUrl}Gio_Hang/{userId}") ?? new GioHang();
        }

        public async Task<bool> AddToCartAsync(object cartItem)// dùng thêm dữ liệu vào giỏ hàng của khách hàng nhóe
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}Gio_Hang/AddToCart", cartItem);
            return response.IsSuccessStatusCode;
        }

    }
}
