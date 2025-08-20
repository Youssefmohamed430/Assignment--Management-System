using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.DataLayer;
using Assignment__Management_System.Models;

namespace Assignment__Management_System.Services
{
    public interface IAuthService
    {
        Task<AuthModel> AddUserAsync(UserDto model);
        Task<AuthModel> LoginAsync(TokenRequestModel model);
    }
}
