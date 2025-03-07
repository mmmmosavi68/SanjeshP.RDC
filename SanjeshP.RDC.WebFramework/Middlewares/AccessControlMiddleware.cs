using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Data;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SanjeshP.RDC.WebFramework.Middlewares
{
    public class AccessControlMiddleware
    {
        private readonly RequestDelegate _next;

        public AccessControlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            // دریافت UserId از Claim
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                // دریافت Area, Controller و Action از RouteValues
                var area = context.Request.RouteValues["area"]?.ToString();
                var controller = context.Request.RouteValues["controller"]?.ToString();
                var action = context.Request.RouteValues["action"]?.ToString();

                // بررسی دسترسی شخصی
                //var hasPersonalAccess = dbContext.UserAccessMenus
                //    .Any(uam => uam.UserId == userId
                //                && uam.Menu.Area == area
                //                && uam.Menu.ControllerName == controller
                //                && uam.Menu.ActionName == action);

                //// بررسی دسترسی گروهی
                //var hasGroupAccess = dbContext.GroupAccessMenus
                //    .Any(gam => gam.Group.GroupUsers.Any(gu => gu.UserId == userId)
                //                && gam.Menu.Area == area
                //                && gam.Menu.ControllerName == controller
                //                && gam.Menu.ActionName == action);

                // اگر کاربر هیچ دسترسی‌ای ندارد
                //if (!hasPersonalAccess && !hasGroupAccess)
                //{
                //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //    await context.Response.WriteAsync("Access Denied: You do not have permission to access this resource.");
                //    return;
                //}
            }

            // ادامه درخواست به Middleware بعدی
            await _next(context);
        }
    }

}

//public List<Menu> GetUserAccessibleMenus(Guid userId)
//{
//    // دسترسی‌های فردی
//    var individualMenus = _context.UserAccessMenus
//        .Where(uam => uam.UserId == userId)
//        .Select(uam => uam.Menu);

//    // دسترسی‌های گروهی
//    var groupMenus = _context.GroupAccessMenus
//        .Where(gam => gam.Group.GroupUsers.Any(gu => gu.UserId == userId))
//        .Select(gam => gam.Menu);

//    return individualMenus.Union(groupMenus).ToList();
//}

//public bool HasAccess(Guid userId, string area, string controller, string action)
//{
//    // دسترسی‌های فردی کاربر
//    var userAccess = _context.UserAccessMenus
//        .Include(uam => uam.Menu)
//        .Any(uam => uam.UserId == userId
//            && uam.Menu.Area == area
//            && uam.Menu.ControllerName == controller
//            && uam.Menu.ActionName == action);

//    // دسترسی‌های گروهی کاربر
//    var groupAccess = _context.GroupAccessMenus
//        .Include(gam => gam.Menu)
//        .Include(gam => gam.Group)
//        .Any(gam => gam.Group.GroupUsers.Any(gu => gu.UserId == userId)
//            && gam.Menu.Area == area
//            && gam.Menu.ControllerName == controller
//            && gam.Menu.ActionName == action);

//    return userAccess || groupAccess;
//}