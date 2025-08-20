using Assignment__Management_System.DataLayer;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Assignment__Management_System.Factories
{
    public class AuthModelFactory
    {
        public AuthModel CreateAuthModel(string id,string username,string email,DateTime expiresOn,List<string> roles,string JWTSecurityToken )
        {
            return new AuthModel()
            {
                ID = id,
                Username = username,
                Email = email,
                IsAuthenticated = true,
                ExpiresOn = expiresOn,
                Roles = roles,
                Token = JWTSecurityToken,
            };
        }
    }
}
