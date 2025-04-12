using Assignment__Management_System.Models;

namespace Assignment__Management_System.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(TokenRequestModel model);
    }
}
