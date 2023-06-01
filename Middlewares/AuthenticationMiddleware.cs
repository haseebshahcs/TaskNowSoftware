using System.Security.Claims;
using System.Text;
using TaskNowSoftware.Services;

namespace TaskNowSoftware.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthManagerConfig _authManagerConfig;

        // Dependency Injection
        public AuthenticationMiddleware(RequestDelegate next, AuthManagerConfig authManagerConfig)
        {
            _next = next;
            _authManagerConfig = authManagerConfig;
        }

        public async Task Invoke(HttpContext context)
        {
            //Reading the AuthHeader which is signed with JWT
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _authManagerConfig.GetUserIdFromToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userId;
            }

            //Pass to the next middleware
            await _next(context);
        }
    }
}
