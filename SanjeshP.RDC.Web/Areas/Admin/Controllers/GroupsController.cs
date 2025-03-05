using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Data.Contracts.Groups;
using SanjeshP.RDC.Data.Contracts.Menu;
using SanjeshP.RDC.Data.Repositories;
using SanjeshP.RDC.Entities.Group;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Group;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GroupsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ILogger<UsersController> _logger;
        private readonly IGroupsRepository _groupsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IEFRepository<UserGroup> _eFRepositoryUserGroup;
        private readonly IMenuRepository _menuRepository;
        private readonly IAccessMenuRepository _accessMenuRepository;
        private readonly IView_UserMenubarRepository _view_UserMenubarRepository;
        private readonly IAccessMenusGroupRepository _accessMenusGroupRepository;

        public GroupsController(IMapper mapper
                                , ICompositeViewEngine viewEngine
                                , ILogger<UsersController> logger
                                , IGroupsRepository groupsRepository
                                , IUserRepository userRepository
                                , IUserTokenRepository userTokenRepository
                                , IEFRepository<UserGroup> eFRepositoryUserGroup
                                , IMenuRepository menuRepository
                                , IAccessMenuRepository accessMenuRepository
                                , IView_UserMenubarRepository view_UserMenubarRepository
                                , IAccessMenusGroupRepository accessMenusGroupRepository
                                )
        {
            _mapper = mapper;
            _viewEngine = viewEngine;
            _logger = logger;
            _groupsRepository = groupsRepository;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _eFRepositoryUserGroup = eFRepositoryUserGroup;
            this._menuRepository = menuRepository;
            this._accessMenuRepository = accessMenuRepository;
            this._view_UserMenubarRepository = view_UserMenubarRepository;
            this._accessMenusGroupRepository = accessMenusGroupRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GroupsList()
        {

            var groups = _groupsRepository.GetAll();
            var groupSelectDtos = _mapper.Map<IEnumerable<GroupSelectDto>>(groups);
            return PartialView("GroupsList", groupSelectDtos);
        }

        [HttpGet]
        public IActionResult CreateGroup()
        {

            return PartialView("CreateGroup");
        }

        [HttpPost]
        public async Task<ApiResult> CreateGroup([Bind("UserGroupText,IsActive")] GroupSelectDto groupSelectDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(groupSelectDto.UserGroupText))
            {
                ModelState.AddModelError("UserGroupText", "لطفا نامگروه را وارد کنید.");
                return new BadRequestObjectResult(ModelState);
            }
            var existingGroup = await _groupsRepository.Table.AsNoTracking().FirstOrDefaultAsync(up => up.UserGroupText == groupSelectDto.UserGroupText.Trim(), cancellationToken);
            if (existingGroup != null)
            {
                ModelState.AddModelError("UserGroupText", "نام گروه تکراری است.");
                return new BadRequestObjectResult(ModelState);
            }
            var curentUserToken = User.Identity.FindFirstValue("Token");
            UserToken token = await _userTokenRepository.GetByIdAsync(new Guid(curentUserToken), cancellationToken);

            Group group = new Group()
            {
                Id = Guid.NewGuid(),
                UserGroupText = groupSelectDto.UserGroupText.Trim(),
                CreatorId = token.UserId,
                HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                IsActive = groupSelectDto.IsActive,
                IsDelete = false,
                CreateDate = DateTime.Now,
            };
            _groupsRepository.Add(group);

            return Ok();
        }

        public IActionResult EditGroup(Guid groupId)
        {
            var group = _groupsRepository.GetById(groupId);
            var groupDto = _mapper.Map<GroupSelectDto>(group);
            return PartialView("CreateGroup", groupDto);
        }

        [HttpPost]
        public async Task<ApiResult> EditGroup([Bind("Id,UserGroupText,IsActive")] GroupSelectDto groupSelectDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(groupSelectDto.UserGroupText))
            {
                ModelState.AddModelError("UserGroupText", "لطفا نامگروه را وارد کنید.");
                return new BadRequestObjectResult(ModelState);
            }
            var group = await _groupsRepository.Table.AsNoTracking().FirstOrDefaultAsync(up => up.Id == groupSelectDto.Id, cancellationToken);

            if (group.UserGroupText != groupSelectDto.UserGroupText.Trim())
            {
                var existingGroup = await _groupsRepository.Table.AsNoTracking().FirstOrDefaultAsync(up => up.UserGroupText == groupSelectDto.UserGroupText.Trim(), cancellationToken);
                if (existingGroup != null)
                {
                    ModelState.AddModelError("UserGroupText", "نام گروه تکراری است.");
                    return new BadRequestObjectResult(ModelState);
                }
            }
            var curentUserToken = User.Identity.FindFirstValue("Token");
            UserToken token = await _userTokenRepository.GetByIdAsync(new Guid(curentUserToken), cancellationToken);

            group.UserGroupText = groupSelectDto.UserGroupText.Trim();
            group.CreatorId = token.UserId;
            group.HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            group.IsActive = groupSelectDto.IsActive;
            group.CreateDate = DateTime.Now;
            _groupsRepository.Update(group);

            return Ok();
        }

        [HttpPost]
        public async Task<ApiResult> DeleteGroup(Guid groupId, CancellationToken cancellationToken)
        {
            var group = await _groupsRepository.Table.AsNoTracking().FirstOrDefaultAsync(up => up.Id == groupId, cancellationToken);
            if (group == null)
            {
                ModelState.AddModelError("GroupError", "گروه یافت نشد.");
                return new BadRequestObjectResult(ModelState);
            }
            var hasUsersInGroup = await _eFRepositoryUserGroup.Table.AsNoTracking().AnyAsync(up => up.GroupId == groupId, cancellationToken);
            if (hasUsersInGroup)
            {
                ModelState.AddModelError("UserGroupError", "در این گروه کاربر وجود دارد و امکان حذف وجود ندارد.");
                return new BadRequestObjectResult(ModelState);
            }

            group.IsDelete = true;
            await _groupsRepository.UpdateAsync(group, cancellationToken);

            return Ok();
        }

        public async Task<IActionResult> UsersGroup(Guid groupId,CancellationToken cancellationToken)
        {
            ViewData["groupId"] = groupId;
            return PartialView("UsersGroup");
        }

        public async Task<IActionResult> UsersGroupList(Guid groupId, CancellationToken cancellationToken)
        {
            //var usersGroup = await _eFRepositoryUserGroup.TableNoTracking.ProjectTo<UserGroupSelectDto>(_mapper.ConfigurationProvider)
            //  .Where(u => u.IsDelete == false && u.GroupId.Equals(groupId))
            //  .ToListAsync(cancellationToken);

            var usersGroup = await _eFRepositoryUserGroup.TableNoTracking
                                        .ProjectTo<UserGroupSelectDto>(_mapper.ConfigurationProvider)
                                        .Where(u => u.IsDelete == false && u.GroupId.Equals(groupId))
                                        .ToListAsync(cancellationToken);

            return PartialView("UsersGroupList", usersGroup);
        }

        #region دریافت فهرست منو بر اساس سطح دسترسی کاربر و نمای سطح دسترسی گروه انتخابی
        public IActionResult GroupAccessMenu(Guid groupId)
        {
            ViewData["groupId"] = groupId;
            return PartialView("GroupAccessMenu");
        }
        public async Task<ActionResult<string>> GetGroupAccessMenuItem(Guid groupid, CancellationToken cancellationToken)
        {
            var curentUserToken = User.Identity.FindFirstValue("Token");
            UserToken token = await _userTokenRepository.GetByIdAsync(new Guid(curentUserToken), cancellationToken);

            #region Convert ListMenu to ListMenuUserAccessDto for use jstree
            List<Menu> listMenus = await _menuRepository.GetAllMenu(cancellationToken);
            List<ListMenuUserAccessDto> listMenuUserAccessDtos = new List<ListMenuUserAccessDto>();
            foreach (var item in listMenus)
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

            #region Add Group Access item
            List<View_UserMenubar> view_GrpoupMenubars = await _view_UserMenubarRepository.GetGroupSelectedAccessMenuByUserCurrentIdAndGroupSelectedID(token.UserId, groupid, cancellationToken);
            foreach (var item in view_GrpoupMenubars)
            {
                foreach (var item2 in listMenuUserAccessDtos)
                {
                    if (item.Id.Equals(item2.id))
                    {
                        item2.Person_Checkecd = item.Person_Checkecd;
                        item2.Group_Checkecd = item.Group_Checkecd;
                    }
                }
            }
            #endregion

            #region Add options jstree
            foreach (var item in listMenuUserAccessDtos)
            {
                if (item.Group_Checkecd == true)
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
                        //listMenuUserAccessStateDto.disabled = item.Group_Checkecd;
                        item.state = listMenuUserAccessStateDto;
                    }
                    else
                    {
                        ListMenuUserAccessStateDto listMenuUserAccessStateDto = new ListMenuUserAccessStateDto();
                        listMenuUserAccessStateDto.selected = false;
                        listMenuUserAccessStateDto.opened = false;
                        //listMenuUserAccessStateDto.disabled = item.Group_Checkecd;
                        item.state = listMenuUserAccessStateDto;
                    }
                }
            }
            #endregion

            var json = JsonConvert.SerializeObject(listMenuUserAccessDtos);
            return (json);
        }
        #endregion پایان

        #region افزودن و اصلاح سطح دسترسی گروه انتخابی
        public async Task<ApiResult> Modify_SelectedNodes_SelectedGroup(string list, Guid groupId, CancellationToken cancellationToken)
        {
            List<AccessMenusGroup> userAccessMenusGroup = await _accessMenusGroupRepository.GetAllByGroupIdAsync(groupId, cancellationToken);
            //Remove all access
            if (list is null)
            {
                foreach (var item in userAccessMenusGroup)
                {
                    item.IsDelete = true;
                    await _accessMenusGroupRepository.UpdateAsync(item, cancellationToken);
                }
            }
            else
            {
                string[] strArray = list.Split(",");
                foreach (var item in strArray)
                {
                    bool isExist = userAccessMenusGroup.Any(ua => ua.ListMenuId.Equals(new Guid(item)));
                    if (!isExist)
                    {
                        AccessMenusGroup NewAccess = new AccessMenusGroup()
                        {
                            ListMenuId = new Guid(item),
                            GroupId = groupId,
                            Creator = new Guid(User.Identity.FindFirstValue(ClaimTypes.NameIdentifier)),
                            IsDelete = false,
                            IsActive = true,
                            HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                        };
                    await _accessMenusGroupRepository.AddAsync(NewAccess, cancellationToken);
                    }
                }
                foreach (var item in userAccessMenusGroup)
                {
                    bool isExist = strArray.Any(ua => ua.Equals(item.ListMenuId.ToString()));
                    if (!isExist)
                    {
                        item.IsDelete = true;
                        item.IsActive = false;
                        await _accessMenusGroupRepository.UpdateAsync(item, cancellationToken);
                    }
                    else if (isExist && item.IsDelete == true)
                    {
                        item.IsDelete = false;
                        item.IsActive = true;
                        await _accessMenusGroupRepository.UpdateAsync(item, cancellationToken);
                    }
                }
            }
            return Ok();
        }
        #endregion پایان
    }
}
