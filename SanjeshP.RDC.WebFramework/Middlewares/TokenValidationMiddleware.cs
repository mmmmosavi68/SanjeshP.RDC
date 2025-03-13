using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Data;
using SanjeshP.RDC.Services.TokenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SanjeshP.RDC.WebFramework.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenService _tokenService;

        public TokenValidationMiddleware(RequestDelegate next, TokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // استثنا برای مسیرهای عمومی مثل ورود و ثبت‌نام
            if (context.Request.Path.StartsWithSegments("/Account/Login", StringComparison.OrdinalIgnoreCase) ||
                context.Request.Path.StartsWithSegments("/Account/Register", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context); // ادامه به Middleware بعدی
                return;
            }

            // بررسی توکن فقط در مسیرهای محافظت شده
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                // بررسی صحت توکن
                if (Guid.TryParse(token, out Guid tokenGuid))
                {
                    var isValid = await _tokenService.ValidateTokenAsync(tokenGuid, context.RequestAborted);
                    if (!isValid)
                    {
                        // هدایت کاربر به صفحه ورود در صورت نامعتبر بودن توکن
                        context.Response.Redirect("/Account/Login");
                        return;
                    }
                }
                else
                {
                    // اگر توکن قابل تبدیل به Guid نبود
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }
            //else
            //{
            //    // اگر توکنی وجود نداشت، کاربر به صفحه ورود هدایت شود
            //    context.Response.Redirect("/Account/Login");
            //    return;
            //}

            // ادامه پردازش برای درخواست‌های معتبر
            await _next(context);
        }
    }
}