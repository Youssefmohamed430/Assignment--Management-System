using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.DataLayer;
using Assignment__Management_System.Models;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterUserAsync(UserDto model);
        Task<AuthModel> LoginAsync(TokenRequestModel model);
        Task<AuthModel> RefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<AuthModel> CreateAuthenticatedResponseAsync(ApplicationUser user);
    }
}
