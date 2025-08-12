using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Helpers;
using Assignment__Management_System.Models;
using Assignment__Management_System.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Assignment__Management_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager ;
        private readonly JWTService _jwtservice ;
        public AuthService(UserManager<ApplicationUser> _userManager, JWTService jwtservice)
        {
            this.userManager = _userManager;
            this._jwtservice = jwtservice;
        }

        public async Task<AuthModel> AddUserAsync([FromBody] UserDto model)
        {
            if(await userManager.FindByNameAsync(model.UserName) is not null) 
                return new AuthModel(){ Message = "User Name Is Already Registerd"};

            if(await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel(){ Message = "Email Is Already Registerd"};

            ApplicationUser user = new ApplicationUser() 
            {
                UserName = model.UserName,
                Name = model.Name,
                Email = model.Email,
            };

            var result = await userManager.CreateAsync(user,model.Password);

            if(!result.Succeeded)
            {
                var errors = "";

                foreach (var error in result.Errors)
                    errors += $"{error.Description}, ";

                return new AuthModel() { Message = errors };
            }

            await userManager.AddToRoleAsync(user, model.Role);



            var JWTSecurityToken = await _jwtservice.CreateJwtToken(user);

            return new AuthModel()
            {
                ID = user.Id,
                Username = model.UserName,
                Email = model.Email,
                IsAuthenticated = true,
                ExpiresOn = JWTSecurityToken.ValidTo,
                Roles = new List<string> { model.Role },
                Token = new JwtSecurityTokenHandler().WriteToken(JWTSecurityToken),
            };

        }
        public async Task<AuthModel> LoginAsync([FromBody] TokenRequestModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null || !await userManager.CheckPasswordAsync(user,model.password))
                return new AuthModel() { Message = "User Name or Password is incorrect!"};
            
            var JWTSecurityToken = await _jwtservice.CreateJwtToken(user);

            return new AuthModel()
            {
                ID = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                ExpiresOn = JWTSecurityToken.ValidTo,
                Roles = JWTSecurityToken.Claims
                    .Where(x => x.Type == "roles")
                    .Select(x => x.Value)
                    .ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(JWTSecurityToken),
            };
        }

    }
}
