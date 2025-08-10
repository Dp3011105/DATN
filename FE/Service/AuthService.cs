using FE.Models;
using FE.Service.IService;
using System.Text;
using System.Text.Json;

namespace FE.Service
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7169/");
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<LoginResponse> LoginAsync(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception("Tên người dùng hoặc mật khẩu không đúng.");
            }
            else
            {
                throw new Exception("Đăng nhập thất bại.");
            }
        }
    }
}
