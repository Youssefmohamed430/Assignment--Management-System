using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Assignment__Management_System.Services
{
    public class AdminServices : IAdminService
    {
        private ILogger<AdminServices> _logger;
        private UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public AdminServices(ILogger<AdminServices> logger, UserManager<ApplicationUser> userManager, IAuthService authService)
        {
            this._logger = logger;
            this._userManager = userManager;
            _authService = authService;
        }

        public async Task<AuthModel> AddAdmin(UserDto model)
        {
            _logger.LogInformation($"Login attempt for: {model.UserName}");

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel() { Message = "User Name Is Already Registerd" };

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel() { Message = "Email Is Already Registerd" };

            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.UserName,
                Name = model.Name,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = "";

                foreach (var error in result.Errors)
                    errors += $"{error.Description}, ";

                return new AuthModel() { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            return await _authService.CreateAuthenticatedResponseAsync(user);
        }
    }
}
