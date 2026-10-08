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
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Assignment__Management_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager ;
        private readonly JWTService _jwtservice ;
        private readonly AppDbContext context;
        private readonly ILogger<AuthService> _logger;
        private readonly JWT _jwtOptions;

        public AuthService(AppDbContext _context, UserManager<ApplicationUser> _userManager, JWTService jwtservice,
            ILogger<AuthService> logger, Microsoft.Extensions.Options.IOptions<JWT> jwtOptions)
        {
            this.userManager = _userManager;
            this._jwtservice = jwtservice;
            this.context = _context;
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<AuthModel> RegisterUserAsync(UserDto model)
        {
            _logger.LogInformation("User registration started for role {Role}", model.Role);

            if (await userManager.FindByNameAsync(model.UserName) is not null)
            {
                _logger.LogWarning("User registration rejected because the username already exists.");
                return new AuthModel() { Message = "User Name Is Already Registerd" };
            }

            if (await userManager.FindByEmailAsync(model.Email) is not null)
            {
                _logger.LogWarning("User registration rejected because the email already exists.");
                return new AuthModel() { Message = "Email Is Already Registerd" };
            }

            ApplicationUser user = new ApplicationUser() 
            {
                UserName = model.UserName,
                Name = model.Name,
                Email = model.Email,
            };

            var result = await userManager.CreateAsync(user,model.Password);

            if(!result.Succeeded)
            {
                _logger.LogWarning(
                    "User registration failed validation for role {Role}; identity returned {ErrorCount} errors.",
                    model.Role,
                    result.Errors.Count());
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

            _logger.LogInformation("User {UserId} registered with role {Role}.", user.Id, model.Role);
            return await CreateAuthenticatedResponseAsync(user);
        }
        public async Task<AuthModel> LoginAsync(TokenRequestModel model) 
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null || !await userManager.CheckPasswordAsync(user,model.password))
            {
                _logger.LogWarning("Login failed because the credentials were invalid.");
                return new AuthModel() { Message = "User Name or Password is incorrect!"};
            }
            
            _logger.LogInformation("User {UserId} logged in successfully.", user.Id);
            return await CreateAuthenticatedResponseAsync(user);
        }

        public async Task<AuthModel> RefreshTokenAsync(string rawRefreshToken)
        {
            var now = DateTime.UtcNow;
            var tokenHash = HashRefreshToken(rawRefreshToken);
            var current = await context.RefreshTokens
                .Include(token => token.User)
                .SingleOrDefaultAsync(token => token.TokenHash == tokenHash);

            if (current is null || current.RevokedAt.HasValue || current.ExpiresAt <= now)
            {
                _logger.LogWarning("Refresh token request rejected because the token is invalid, expired, or revoked.");
                return new AuthModel { Message = "Refresh token is invalid or expired." };
            }

            var nextRawToken = GenerateRefreshToken();
            var nextHash = HashRefreshToken(nextRawToken);
            var nextExpiresAt = now.AddDays(Math.Max(1, _jwtOptions.RefreshTokenExpirationDays));
            current.RevokedAt = now;
            current.ReplacedByTokenHash = nextHash;
            context.RefreshTokens.Add(new RefreshToken
            {
                UserId = current.UserId,
                TokenHash = nextHash,
                CreatedAt = now,
                ExpiresAt = nextExpiresAt
            });

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogWarning("Concurrent refresh token use rejected for user {UserId}.", current.UserId);
                return new AuthModel { Message = "Refresh token has already been used." };
            }

            var jwt = await _jwtservice.CreateJwtToken(current.User);
            var roles = await userManager.GetRolesAsync(current.User);
            _logger.LogInformation("Access token refreshed for user {UserId}.", current.UserId);
            return CreateAuthModel(current.User, roles.ToList(), jwt, nextRawToken, nextExpiresAt);
        }

        public async Task RevokeRefreshTokenAsync(string rawRefreshToken)
        {
            if (string.IsNullOrWhiteSpace(rawRefreshToken))
                return;

            var hash = HashRefreshToken(rawRefreshToken);
            var token = await context.RefreshTokens.SingleOrDefaultAsync(item => item.TokenHash == hash);
            if (token is null || token.RevokedAt.HasValue)
                return;

            token.RevokedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            _logger.LogInformation("Refresh token revoked for user {UserId}.", token.UserId);
        }

        public async Task<AuthModel> CreateAuthenticatedResponseAsync(ApplicationUser user)
        {
            var now = DateTime.UtcNow;
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiresOn = now.AddDays(Math.Max(1, _jwtOptions.RefreshTokenExpirationDays));
            context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = HashRefreshToken(refreshToken),
                CreatedAt = now,
                ExpiresAt = refreshTokenExpiresOn
            });
            await context.SaveChangesAsync();

            var jwt = await _jwtservice.CreateJwtToken(user);
            var roles = await userManager.GetRolesAsync(user);
            return CreateAuthModel(user, roles.ToList(), jwt, refreshToken, refreshTokenExpiresOn);
        }

        private static AuthModel CreateAuthModel(
            ApplicationUser user,
            List<string> roles,
            JwtSecurityToken jwt,
            string refreshToken,
            DateTime refreshTokenExpiresOn)
        {
            return new AuthModelFactory().CreateAuthModel(
                user.Id,
                user.UserName ?? string.Empty,
                user.Email ?? string.Empty,
                jwt.ValidTo,
                roles,
                new JwtSecurityTokenHandler().WriteToken(jwt),
                refreshToken,
                refreshTokenExpiresOn);
        }

        private static string GenerateRefreshToken() =>
            Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(64));

        private static string HashRefreshToken(string token) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}
