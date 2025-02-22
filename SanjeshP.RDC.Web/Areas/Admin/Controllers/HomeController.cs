using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMM.Olympiad.Web.Areas.UserArea.Models;
using Newtonsoft.Json;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Menubar
        //[ViewComponent(Name = "MenuBar")]
        //public class MenuBarViewComponent : ViewComponent
        //{
        //    public async Task<IViewComponentResult> InvokeAsync()
        //    {
        //        var _view_UserMenubar = JsonConvert.DeserializeObject<List<View_UserMenubar>>(UserClaimsPrincipal.FindFirst("View_UserMenubar")?.Value);
        //        if (_view_UserMenubar == null)
        //        {
        //            // Handle the case where _view_UserMenubar is null
        //            return View(new List<ListMenuDto>());
        //        }

        //        List<ListMenuDto> FinalList = new List<ListMenuDto>();
        //        foreach (var item in _view_UserMenubar)
        //        {
        //            if (item.ParentId == null)
        //            {
        //                FinalList.Add(new ListMenuDto
        //                {
        //                    CssClass = item.CssClass,
        //                    Icon = item.Icon,
        //                    Id = item.Id,
        //                    Link = item.Link,
        //                    PageCode = item.PageCode,
        //                    ParentId = item.ParentId,
        //                    ShowMenu = item.ShowMenu,
        //                    Title = item.Title,
        //                    IsDelete = item.Person_Checkecd,
        //                    ActionName = item.ActionName,
        //                    ControllerName = item.ControllerName,
        //                    children = GetChildMenu(item.Id, _view_UserMenubar)
        //                });
        //            }
        //        }

        //        return View(FinalList);
        //    }

        //    private List<ListMenuDto> GetChildMenu(Guid _id, List<SanjeshP.RDC.Entities.Menu.View_UserMenubar> _List)
        //    {
        //        List<ListMenuDto> FinalList = new List<ListMenuDto>();
        //        foreach (var item in _List)
        //        {
        //            if (item.ParentId == _id)
        //            {
        //                FinalList.Add(new ListMenuDto
        //                {
        //                    CssClass = item.CssClass,
        //                    Icon = item.Icon,
        //                    Id = item.Id,
        //                    Link = item.Link,
        //                    PageCode = item.PageCode,
        //                    ParentId = item.ParentId,
        //                    ShowMenu = item.ShowMenu,
        //                    Title = item.Title,
        //                    ActionName = item.ActionName,
        //                    ControllerName = item.ControllerName,
        //                    children = GetChildMenu(item.Id, _List)
        //                });
        //            }
        //        }
        //        return FinalList;
        //    }
        //}
        #endregion
    }
}
