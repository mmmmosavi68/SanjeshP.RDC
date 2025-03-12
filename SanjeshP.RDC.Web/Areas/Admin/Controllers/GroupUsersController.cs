using Microsoft.AspNetCore.Mvc;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Entities.Group;
using SanjeshP.RDC.WebFramework.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using SanjeshP.RDC.Data.Contracts.Common;
using SanjeshP.RDC.Data.Contracts.Groups;
using SanjeshP.RDC.Data.Contracts.Menu;
using SanjeshP.RDC.Data.Contracts.Menus;
using SanjeshP.RDC.Data.Contracts.Users;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User;
using SanjeshP.RDC.Web.Areas.Admin.ViewModels.GroupUsers;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Group;
using AutoMapper.QueryableExtensions;
using SanjeshP.RDC.Web.Areas.Admin.ViewModels.User;

namespace SanjeshP.RDC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GroupUsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ILogger<UsersController> _logger;
        private readonly IGroupRepository _groupsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IEntityFrameworkRepository<GroupUsers> _eFRepositoryUserGroup;
        private readonly IMenusRepository _menuRepository;
        private readonly IAccessMenusRepository _accessMenuRepository;
        private readonly IViewUserMenubarRepository _view_UserMenubarRepository;
        private readonly IAccessMenusGroupRepository _accessMenusGroupRepository;

        public GroupUsersController(IMapper mapper
                                , ICompositeViewEngine viewEngine
                                , ILogger<UsersController> logger
                                , IGroupRepository groupsRepository
                                , IUserRepository userRepository
                                , IUserTokenRepository userTokenRepository
                                , IEntityFrameworkRepository<GroupUsers> eFRepositoryUserGroup
                                , IMenusRepository menuRepository
                                , IAccessMenusRepository accessMenuRepository
                                , IViewUserMenubarRepository view_UserMenubarRepository
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
            _menuRepository = menuRepository;
            _accessMenuRepository = accessMenuRepository;
            _view_UserMenubarRepository = view_UserMenubarRepository;
            _accessMenusGroupRepository = accessMenusGroupRepository;
        }
        public async Task<IActionResult> GroupUsersIndex(Guid groupId, CancellationToken cancellationToken)
        {
            ViewData["groupId"] = groupId;
            return PartialView("GroupUsersIndex");
        }

        public async Task<IActionResult> GroupUsersList(Guid groupId, CancellationToken cancellationToken)
        {
            try
            {

                ViewData["groupId"] = groupId;

                //var groupUserDtos = await _eFRepositoryUserGroup.TableNoTracking
                //                                    .Where(u => !u.IsDeleted && u.GroupId == groupId)
                //                                    .Select(u => new GroupUserDto
                //                                    {
                //                                        UserId = u.UserId.Value,
                //                                        UserName = u.User.UserName,
                //                                        FullName = u.User.UserProfiles.Any()
                //                                                   ? u.User.UserProfiles.FirstOrDefault().FirstName + " " + u.User.UserProfiles.FirstOrDefault().LastName
                //                                                   : "",
                //                                        NationalCode = u.User.UserProfiles.Any()
                //                                                       ? u.User.UserProfiles.FirstOrDefault().NationalCode
                //                                                       : "",
                //                                        GroupId = u.GroupId.Value,
                //                                        GroupName = u.Group.GroupName,
                //                                        CreateDate = u.CreatedDate
                //                                    })
                //                                    .ToListAsync(cancellationToken);

                //var groupUserViewModels = groupUserDtos.Select(dto => new GroupUserViewModel
                //{
                //    UserId = dto.UserId,
                //    UserName = dto.UserName,
                //    FullName = dto.FullName,
                //    NationalCode = dto.NationalCode,
                //    GroupName = dto.GroupName,
                //    IsActive = dto.IsActive,
                //    CreateDate = dto.CreateDate
                //}).ToList();

                var groupUsers = await _eFRepositoryUserGroup.TableNoTracking
                                            .ProjectTo<UserGroupSelectDto>(_mapper.ConfigurationProvider)
                                            .Where(u => u.IsDelete == false && u.GroupId.Equals(groupId))
                                            .ToListAsync(cancellationToken);

                return PartialView("GroupUsersList", groupUsers);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IActionResult> UsersNotInGroup(Guid groupId, CancellationToken cancellationToken)
        {
            ViewData["groupId"] = groupId;
            // گرفتن لیست تمامی کاربران
            var users = await _userRepository.GetAllUsersNoTrackingAsync(cancellationToken);

            List<UserViewModel> allUsers = users.Select(user => new UserViewModel
            {
                UserId = user.Id,
                FirstName = user.UserProfiles.Select(p => p.FirstName).FirstOrDefault(),
                LastName = user.UserProfiles.Select(p => p.LastName).FirstOrDefault(),
                NationalCode = user.UserProfiles.Select(p => p.NationalCode).FirstOrDefault(),
                UserName = user.UserName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                UserTypeTitle = user.UserRoles.Select(p => p.Role.RoleNameFa).Last(),
                RoleId = user.UserRoles.Select(p => p.RoleId).Last(),
                IsActive = user.IsActive,

            }).ToList();

            // گرفتن لیست کاربرانی که در گروه انتخابی هستند
            var usersInGroup = await _eFRepositoryUserGroup.TableNoTracking
                                       .Where(ug => ug.GroupId.Equals(groupId) && ug.IsActive == true && ug.IsDeleted == false)
                                       .Select(ug => ug.UserId)
                                       .ToListAsync(cancellationToken);
            // فیلتر کردن کاربرانی که در گروه انتخابی نیستند
            var usersNotInGroup = allUsers
                                    .Where(u => !usersInGroup.Contains(u.UserId))
                                    .ToList();
            return PartialView("UsersNotInGroup", usersNotInGroup);
        }

        [HttpPost]
        public async Task<ApiResult> DeleteUserGroup(Guid id, CancellationToken cancellationToken)
        {
            var userGroup = await _eFRepositoryUserGroup.Table.AsNoTracking().FirstOrDefaultAsync(up => up.Id == id, cancellationToken);
            if (userGroup == null)
            {
                ModelState.AddModelError("GroupError", "ردیفی یافت نشد.");
                return new BadRequestObjectResult(ModelState);
            }

            userGroup.IsActive = false;
            userGroup.IsDeleted = true;
            await _eFRepositoryUserGroup.UpdateAsync(userGroup, cancellationToken);

            return Ok();
        }

        [HttpPost]
        public async Task<ApiResult> AddUserGroup(Guid userid, Guid groupid, CancellationToken cancellationToken)
        {
            var ExistuserGroup = await _eFRepositoryUserGroup.Table.AsNoTracking()
                                        .FirstOrDefaultAsync(up => up.GroupId == groupid && up.UserId == userid && up.IsDeleted == false && up.IsActive == true, cancellationToken);
            if (ExistuserGroup != null)
            {
                ModelState.AddModelError("GroupError", "کاربر در گروه وجود دارد.");
                return new BadRequestObjectResult(ModelState);
            }

            var curentUserToken = User.Identity.FindFirstValue("Token");
            var token = await _userTokenRepository.GetUserTokenByIdAsync(new Guid(curentUserToken), cancellationToken);

            GroupUsers userGroup = new GroupUsers()
            {
                Id = Guid.NewGuid(),
                UserId = userid,
                GroupId = groupid,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                CreatedBy = token.UserId,

            };
            await _eFRepositoryUserGroup.AddAsync(userGroup, cancellationToken);

            return Ok();
        }
    }
}
