using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SanjeshP.RDC.Data.Contracts.Menus;
using SanjeshP.RDC.Data.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SanjeshP.RDC.WebFramework.Middlewares
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            // بررسی `AllowAnonymous` روی اکشن یا کنترلر
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context); // ادامه به Middleware بعدی
                return;
            }

            // بررسی ورود کاربر
            var user = context.User;
            var tokenClaim = user.FindFirst("token")?.Value;

            if (user?.Identity?.IsAuthenticated != true || string.IsNullOrEmpty(tokenClaim))
            {
                await context.SignOutAsync(); // خروج کاربر در صورت عدم احراز هویت
                context.Response.Redirect("/Account/Login");
                return;
            }

            if (Guid.TryParse(tokenClaim, out Guid token))
            {
                var tokenRepository = context.RequestServices.GetRequiredService<IUserTokenRepository>();
                var haToken = await tokenRepository.GetUserTokenByIdAsync(token, context.RequestAborted);

                if (haToken == null || haToken.IsDeleted)
                {
                    await context.SignOutAsync(); // خروج کاربر در صورت توکن نامعتبر
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }
            else
            {
                // اگر توکن معتبر نبود
                await context.SignOutAsync();
                context.Response.Redirect("/Account/Login");
                return;
            }

            // بررسی سطح دسترسی کاربر
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                var userMenubarRepository = context.RequestServices.GetRequiredService<IViewUserMenubarRepository>();
                var result = userMenubarRepository.GetUserAccessMenus(userId, context.RequestAborted);

                var actionName = context.GetRouteValue("action")?.ToString();
                var controllerName = context.GetRouteValue("controller")?.ToString();

                var hasPermission = result.Any(vum =>
                    vum.ControllerName == controllerName &&
                    vum.ActionName == actionName &&
                    vum.Person_Checkecd);

                if (!hasPermission)
                {
                    context.Response.Redirect("/Account/Login"); // هدایت کاربر به صفحه بدون دسترسی
                    return;
                }
            }
            else
            {
                await context.SignOutAsync();
                context.Response.Redirect("/Account/Login");
                return;
            }

            // ادامه درخواست به Middleware بعدی
            await _next(context);
        }
    }

}
