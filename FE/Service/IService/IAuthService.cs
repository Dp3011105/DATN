using FE.Models;
namespace FE.Service.IService
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterModel model);
        Task<LoginResponse> LoginAsync(LoginModel model);
    }
}
