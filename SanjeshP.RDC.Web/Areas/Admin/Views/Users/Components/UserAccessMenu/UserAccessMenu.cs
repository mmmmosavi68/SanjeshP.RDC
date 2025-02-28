using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SanjeshP.RDC.Web.Areas.Admin.Controllers;
using SanjeshP.RDC.Data.Contracts;

namespace SanjeshP.RDC.Web.Areas.Admin.Views.Users.Components.UserAccessMenu
{
    [ViewComponent(Name = "UserAccessMenu")]
    public class UserAccessMenuViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserAccessMenuViewComponent> _logger;
        private readonly IMenuRepository _menuRepository;
        private readonly IView_UserMenubarRepository _view_UserMenubarRepository;

        public UserAccessMenuViewComponent(IMapper mapper
                                , ILogger<UserAccessMenuViewComponent> logger
                                , IMenuRepository menuRepository
                                , IView_UserMenubarRepository view_UserMenubarRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _menuRepository = menuRepository;
            _view_UserMenubarRepository = view_UserMenubarRepository;
        }

        #region Start UserAccessMenu jsTree
        public async Task<IViewComponentResult> InvokeAsync(Guid userid, CancellationToken cancellationToken)
        {
            List<View_UserMenubar> view_UserMenubars = await _view_UserMenubarRepository.GetUserMenuByUser_Id(userid, cancellationToken);
            List<Menu> Menu = await _menuRepository.GetAllMenu(cancellationToken);

            #region Convert ListMenu to ListMenuUserAccessDto for use jstree
            List<ListMenuUserAccessDto> listMenuUserAccessDtos = new List<ListMenuUserAccessDto>();
            foreach (var item in Menu)
            {
                if (item.ParentId == null)
                {
                    listMenuUserAccessDtos.Add(new ListMenuUserAccessDto
                    {
                        id = item.Id,
                        parent = "#",
                        text = item.Title,
                    });
                }
                else
                {
                    listMenuUserAccessDtos.Add(new ListMenuUserAccessDto
                    {
                        id = item.Id,
                        parent = item.ParentId.ToString(),
                        text = item.Title,
                    });
                }
            }
            #endregion

            #region Add User Access item
            foreach (var item in view_UserMenubars)
            {
                foreach (var item2 in listMenuUserAccessDtos)
                {
                    if (item.Id.Equals(item2.id))
                    {
                        item2.Person_Checkecd = item.Person_Checkecd;
                    }
                }
            }
            #endregion

            #region Add options jstree
            foreach (var item in listMenuUserAccessDtos)
            {
                if (item.Person_Checkecd == true)
                {
                    var result = true;
                    foreach (var item2 in listMenuUserAccessDtos)
                    {
                        if (item2.parent == item.id.ToString())
                        {
                            result = false;
                            break;
                        }
                    }
                    if (result)
                    {
                        ListMenuUserAccessStateDto listMenuUserAccessStateDto = new ListMenuUserAccessStateDto();
                        listMenuUserAccessStateDto.selected = true;
                        listMenuUserAccessStateDto.opened = true;
                        item.state = listMenuUserAccessStateDto;
                    }
                    else
                    {
                        ListMenuUserAccessStateDto listMenuUserAccessStateDto = new ListMenuUserAccessStateDto();
                        listMenuUserAccessStateDto.selected = false;
                        listMenuUserAccessStateDto.opened = false;
                        item.state = listMenuUserAccessStateDto;
                    }
                }
            }
            #endregion

            var json = JsonConvert.SerializeObject(listMenuUserAccessDtos);
            return View(json);
        }
        #endregion End UserAccessMenu jsTree
    }
}
