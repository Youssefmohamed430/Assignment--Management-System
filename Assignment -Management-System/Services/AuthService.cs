using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.DataLayer;
using Assignment__Management_System.Factories;
using Assignment__Management_System.Helpers;
using Assignment__Management_System.Models;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Assignment__Management_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager ;
        private readonly JWTService _jwtservice ;
        private readonly AppDbContext context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext _context,UserManager<ApplicationUser> _userManager, JWTService jwtservice, ILogger<AuthService> logger)
        {
            this.userManager = _userManager;
            this._jwtservice = jwtservice;
            this.context = _context;
            _logger = logger;
        }

        public async Task<AuthModel> AddUserAsync([FromBody] UserDto model)
        {
            _logger.LogInformation($"Login attempt for: {model.UserName}");

            if (await FindByNameAsync(model.UserName) is not null)
                return new AuthModel() { Message = "User Name Is Already Registerd" };

            if (await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel() { Message = "Email Is Already Registerd" };

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

            if(model.Role == "Student")
            {
                var student = new Student() { Id = user.Id };
                context.Students.Add(student);
                context.SaveChanges();
            }
            else if(model.Role == "Instructor")
            {
                var instructor = new Instructor() { Id = user.Id };
                context.Instructors.Add(instructor);
                context.SaveChanges();
            }

            var JWTSecurityToken = await _jwtservice.CreateJwtToken(user);

            return new AuthModelFactory()
                .CreateAuthModel(user.Id, model.UserName, model.Email, JWTSecurityToken.ValidTo, new List<string> { model.Role }, new JwtSecurityTokenHandler().WriteToken(JWTSecurityToken));
        }
        public async Task<AuthModel> LoginAsync(TokenRequestModel model) 
        {
            _logger.LogInformation($"Login attempt for: {model.Username}");

            var user = await FindByNameAsync(model.Username);

            if (user == null || !await userManager.CheckPasswordAsync(user,model.password))
                return new AuthModel() { Message = "User Name or Password is incorrect!"};
            
            var JWTSecurityToken = await _jwtservice.CreateJwtToken(user);

            return new AuthModelFactory()
                .CreateAuthModel(user.Id, user.UserName, user.Email, JWTSecurityToken.ValidTo,
                JWTSecurityToken.Claims.Where(x => x.Type == "roles").Select(x => x.Value).ToList()
                , new JwtSecurityTokenHandler().WriteToken(JWTSecurityToken));
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = await context.Users
                         .FirstOrDefaultAsync(u => u.UserName == (string)userName);

            return user;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

    }
}
