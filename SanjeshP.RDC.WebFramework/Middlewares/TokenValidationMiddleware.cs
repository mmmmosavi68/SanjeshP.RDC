using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SanjeshP.RDC.WebFramework.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                //var userToken = await dbContext.UserTokens
                //    .FirstOrDefaultAsync(ut => ut.UserId == userId);

                //if (userToken == null || userToken.ExpirationDate < DateTime.UtcNow)
                //{
                //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //    return;
                //}
            }

            await _next(context);
        }
    }
}