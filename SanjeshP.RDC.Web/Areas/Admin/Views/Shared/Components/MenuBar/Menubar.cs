using Microsoft.AspNetCore.Mvc;
using SanjeshP.RDC.Data.Contracts.Menus;
using SanjeshP.RDC.Data.Contracts.Users;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Web.Areas.Admin.ViewComponents
{
    [ViewComponent(Name = "MenuBar")]
    public class MenuBarViewComponent : ViewComponent
    {
        private readonly IUserTokenRepository _userTokenRepository;

        public readonly IViewUserMenubarRepository _view_UserMenubarRepository;

        public MenuBarViewComponent(IViewUserMenubarRepository view_UserMenubarRepository, IUserTokenRepository userTokenRepository)
        {
            _view_UserMenubarRepository = view_UserMenubarRepository;
            _userTokenRepository = userTokenRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var token = new Guid(HttpContext.User.FindFirst("Token").Value.ToString());
            var userToken = await _userTokenRepository.GetUserTokenByIdAsync(token,HttpContext.RequestAborted);
            var CurentView_UserMenubar = _view_UserMenubarRepository.GetUserAccessMenus(userToken.UserId, HttpContext.RequestAborted);
            if (CurentView_UserMenubar == null)
            {
                // Handle the case where _view_UserMenubar is null
                return View(new List<MenuDto>());
            }

            List<MenuDto> FinalList = new List<MenuDto>();
            foreach (var item in CurentView_UserMenubar)
            {
                if (item.ParentId == null)
                {
                    FinalList.Add(new MenuDto
                    {
                        CssClass = item.CssClass,
                        Icon = item.Icon,
                        Id = item.Id,
                        Link = item.Link,
                        PageCode = item.PageCode,
                        ParentId = item.ParentId,
                        ShowMenu = item.ShowMenu,
                        Title = item.MenuTitle,
                        IsDelete = item.Person_Checkecd,
                        Area = item.Area,
                        ActionName = item.ActionName,
                        ControllerName = item.ControllerName,
                        children = GetChildMenu(item.Id, CurentView_UserMenubar)
                    });
                }
            }

            return View(FinalList);
        }
        private List<MenuDto> GetChildMenu(Guid _id, List<SanjeshP.RDC.Entities.Menu.View_UserMenubar> _List)
        {
            List<MenuDto> FinalList = new List<MenuDto>();
            foreach (var item in _List)
            {
                if (item.ParentId == _id)
                {
                    FinalList.Add(new MenuDto
                    {
                        CssClass = item.CssClass,
                        Icon = item.Icon,
                        Id = item.Id,
                        Link = item.Link,
                        PageCode = item.PageCode,
                        ParentId = item.ParentId,
                        ShowMenu = item.ShowMenu,
                        Title = item.MenuTitle,
                        Area = item.Area,
                        ActionName = item.ActionName,
                        ControllerName = item.ControllerName,
                        children = GetChildMenu(item.Id, _List)
                    });
                }
            }
            return FinalList;
        }
    }
}
