using FE.Models;
namespace FE.Service.IService
{
    public interface IAuthService
    {
        //Task<bool> RegisterAsync(RegisterModel model);
        Task<(bool Success, string Message)> RegisterAsync(RegisterModel model);
        Task<LoginResponse> LoginAsync(LoginModel model);
        Task<(bool Success, string Message)> ForgotPasswordAsync(string email);
    }
}
