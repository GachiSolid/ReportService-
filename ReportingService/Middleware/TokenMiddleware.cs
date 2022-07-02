using Microsoft.AspNetCore.Http;
using ReportingService.Token;
using System;
using System.Threading.Tasks;

namespace ReportingService.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate next;

        public TokenMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IJwtGenerator jwtGenerator)
        {
            bool error = false;
            var token = context.Request.Headers["Authorization"].ToString();

            if (token != "")
            {
                var header = token.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (!header[0].Equals("Bearer") || header.Length != 2)
                    throw new Exception("Broken Authorization Header");

                var jwt = header[1];

                var result = jwtGenerator.ValidateToken(jwt);

                if (!result)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("You need to re-login to access this resource");
                    error = true;
                }
            }

            if (!error)
                await next.Invoke(context);
        }
    }
}
