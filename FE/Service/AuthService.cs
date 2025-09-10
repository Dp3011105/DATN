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

        //public async Task<bool> RegisterAsync(RegisterModel model)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("api/Auth/register", model);
        //    return response.IsSuccessStatusCode;
        //}

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", model);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return (true, responseContent.Trim('"')); // Loại bỏ dấu ngoặc kép nếu API trả về chuỗi JSON
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent.Trim('"')); // Trả về lỗi từ API
            }
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



        public async Task<(bool Success, string Message)> ForgotPasswordAsync(string email)
        {
            var request = new { email }; // Tạo object JSON với trường email
            var response = await _httpClient.PostAsJsonAsync("api/Auth/forgot-password", request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return (true, responseContent.Trim('"')); // Trả về thông báo thành công
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent.Trim('"')); // Trả về lỗi từ API
            }
        }



    }
}
