using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.WebFramework.Middlewares
{
    public class AccessChecker
    {
        private readonly ApplicationDbContext _context;

        public AccessChecker(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool HasAccess(Guid userId, string area, string controller, string action)
        {
            // بررسی دسترسی فردی کاربر
            //var userAccess = _context.UserAccessMenus
            //    .Include(uam => uam.Menu)
            //    .Any(uam => uam.UserId == userId
            //                && uam.Menu.Area == area
            //                && uam.Menu.ControllerName == controller
            //                && uam.Menu.ActionName == action);

            //// بررسی دسترسی گروهی کاربر
            //var groupAccess = _context.GroupAccessMenus
            //    .Include(gam => gam.Menu)
            //    .Include(gam => gam.Group)
            //    .Any(gam => gam.Group.GroupUsers.Any(gu => gu.UserId == userId)
            //                && gam.Menu.Area == area
            //                && gam.Menu.ControllerName == controller
            //                && gam.Menu.ActionName == action);

            //return userAccess || groupAccess; // کاربر اگر دسترسی فردی یا گروهی داشته باشد
            return false;
        }
    }
}