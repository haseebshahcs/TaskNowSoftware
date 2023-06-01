using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TaskNowSoftware.Services
{
    public class AuthManagerConfig
    {
        public const string JWT_AUTH_KEY = "POEMScz6rmVCDyeMbqOwkhoGTJ1ncsjS6749hI2021-POEMScgiyoXKcIrn5ft5FwFOgIrjU6Xm3lc2K2021-POEMS30K6uZMPmvIuNq0uUsU5F7fu7O4miILKr2021";

        private readonly IConfiguration _configuration;
        public AuthManagerConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int? GetUserIdFromToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration.GetSection("Jwt:Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthManagerConfig.JWT_AUTH_KEY)),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
