using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TaskNowSoftware.Services;

namespace TaskNowSoftware.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                #region CHECK ALLOW ANONYMOUS
                var controllerActionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
                if (controllerActionDescriptor != null)
                {
                    // Check if the attribute exists on the action method
                    if (controllerActionDescriptor.MethodInfo?.GetCustomAttributes(inherit: true)?.Any(a => a.GetType().Equals(typeof(AllowAnonymousCustomAttribute))) ?? false)
                        return;
                    // Check if the attribute exists on the controller
                    if (controllerActionDescriptor.ControllerTypeInfo?.GetCustomAttributes(typeof(AllowAnonymousCustomAttribute), true)?.Any() ?? false)
                        return;
                }
                #endregion CHECK ALLOW ANONYMOUS

                var cc = context.HttpContext.Request.Headers["Authorization"];

                var token = cc.FirstOrDefault()?.Split(" ").Last();
                var configuration = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                           .Build();


                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("Jwt:Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthManagerConfig.JWT_AUTH_KEY)),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                //If we come up here, means token is parsed successfully and a valid jwt in variable validatedToken
                //we will let authorization pass, no need of any action (un Auth etc.)

            }
            catch
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
