using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskNowSoftware.Models;

namespace TaskNowSoftware.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _configuration;

        public AuthManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(int userId)
        {
            var signInCredentials = GetSignInCredentials();
            var claims = GetClaims(userId);
            var token = GenerateTokenOptions(signInCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signInCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("LifeTime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signInCredentials
                );

            return token;
        }

        private List<Claim> GetClaims(int userId)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString())
            };
            return claims;
        }

        private SigningCredentials GetSignInCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthManagerConfig.JWT_AUTH_KEY));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}
