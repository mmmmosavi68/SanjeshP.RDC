using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace SanjeshP.RDC.WebFramework.UserAuthorization
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var action_name = context.RouteData.Values["action"].ToString();
            var controller_name = context.RouteData.Values["controller"].ToString();

            //var dd = context.HttpContext.Session.Id;
            //var URL = context.HttpContext.Request.Host.ToString();
            //var Host = context.HttpContext.Request.Path.ToString();
            //string urla = context.ActionDescriptor.DisplayName;

            //var toke = new Guid(context.HttpContext.User.FindFirst("token").Value.ToString());
            //var haToken = context.HttpContext.RequestServices.GetRequiredService<ITokenRe pository>().GetByIdAsync(toke, context.HttpContext.RequestAborted).Result;
            //if (haToken == null)
            //{
            //    await context.HttpContext.SignOutAsync();
            //    context.HttpContext.Response.Redirect("~/Login/Login");
            //}
            //else if (haToken.IsDelete == true)
            //{
            //    await context.HttpContext.SignOutAsync();
            //    context.HttpContext.Response.Redirect("~/Login/Login");
            //}
            //else
            //{
            //    var userid = new Guid(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            //    List<View_UserMenubar> result = context.HttpContext.RequestServices.GetRequiredService<IView_UserMenubarRepository>().GetUserMenuByUser_Id(userid, context.HttpContext.RequestAborted).Result;
            //    //var _view_UserMenubar = JsonConvert.DeserializeObject<List<View_UserMenubar>>(context.HttpContext.User.FindFirst("View_UserMenubar").Value.ToString());
            //    var Check_Permission = result.Any(vum => vum.ControllerName == controller_name
            //                                                && vum.ActionName == action_name
            //                                                && vum.Person_Checkecd.Equals(true));
            //    if (!Check_Permission)
            //    {
            //        context.Result = new RedirectResult("~/Login/NoPermission");
            //    }
            //}
        }
    }
}
