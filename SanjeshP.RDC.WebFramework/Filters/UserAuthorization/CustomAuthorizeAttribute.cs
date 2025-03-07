using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using SanjeshP.RDC.Data.Repositories;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Data.Contracts.Menus;

namespace SanjeshP.RDC.WebFramework.UserAuthorization
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var action_name = context.RouteData.Values["action"]?.ToString();
            var controller_name = context.RouteData.Values["controller"]?.ToString();

            var session = context.HttpContext.Session;
            var host = context.HttpContext.Request.Host.ToString();
            var path = context.HttpContext.Request.Path.ToString();
            var displayName = context.ActionDescriptor.DisplayName;

            var user = context.HttpContext.User;
            var tokenClaim = user.FindFirst("token")?.Value;
            if (Guid.TryParse(tokenClaim, out Guid token))
            {
                //var tokenRepository = context.HttpContext.RequestServices.GetRequiredService<ITokenRepository>();
                //var haToken = tokenRepository.GetByIdAsync(token, context.HttpContext.RequestAborted).Result;
                //if (haToken == null || haToken.IsDelete)
                //{
                //    context.HttpContext.SignOutAsync().GetAwaiter().GetResult();
                //    context.HttpContext.Response.Redirect("~/Login/Login");
                //    return;
                //}
            }
            else
            {
                context.HttpContext.SignOutAsync().GetAwaiter().GetResult();
                context.HttpContext.Response.Redirect("~/Login/Login");
                return;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                var userMenubarRepository = context.HttpContext.RequestServices.GetRequiredService<IViewUserMenubarRepository>();
                List<View_UserMenubar> result = userMenubarRepository.GetUserMenusByUserIdAsync(userId, context.HttpContext.RequestAborted).Result;
                var checkPermission = result.Any(vum => vum.ControllerName == controller_name
                                                        && vum.ActionName == action_name
                                                        && vum.Person_Checkecd);
                if (!checkPermission)
                {
                    context.Result = new RedirectResult("~/Login/NoPermission");
                }
            }
            else
            {
                context.HttpContext.SignOutAsync().GetAwaiter().GetResult();
                context.HttpContext.Response.Redirect("~/Login/Login");
            }
        }
    }
}
