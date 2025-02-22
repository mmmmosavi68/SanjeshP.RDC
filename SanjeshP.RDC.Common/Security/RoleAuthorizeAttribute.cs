using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SanjeshP.RDC.Security
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.Redirect("/login");
            }

            if (context.HttpContext.User.FindFirst("RoleName").Value != "Admin")
            {
                context.HttpContext.Response.Redirect("/login");
            }
        }
    }
}
