using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Convertor;
using SanjeshP.RDC.Data.Contracts.Common;
using SanjeshP.RDC.Data.Contracts.Menus;
using SanjeshP.RDC.Data.Contracts.Users;
using SanjeshP.RDC.Data.Repositories;
using SanjeshP.RDC.Entities.Common;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Security;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfilesRepository;
        private readonly IEntityFrameworkRepository<Role> _eFRepositoryRole;
        private readonly IMenusRepository _menuRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IAccessMenusRepository _accessMenuRepository;
        private readonly IUserRoleRepository _userRoleReository;
        private readonly IViewUserMenubarRepository _view_UserMenubarRepository;

        public UsersController(IMapper mapper
                                , ICompositeViewEngine viewEngine
                                , ILogger<UsersController> logger
                                , IUserRepository userRepository
                                , IUserProfileRepository userProfilesRepository
                                , IEntityFrameworkRepository<Role> eFRepositoryRole
                                , IMenusRepository menuRepository
                                , IViewUserMenubarRepository view_UserMenubarRepository
                                , IUserTokenRepository userTokenRepository
                                , IAccessMenusRepository accessMenuRepository
                                , IUserRoleRepository userRoleReository)
        {
            _mapper = mapper;
            _viewEngine = viewEngine;
            _logger = logger;
            _userRepository = userRepository;
            _userProfilesRepository = userProfilesRepository;
            _eFRepositoryRole = eFRepositoryRole;
            _menuRepository = menuRepository;
            _userTokenRepository = userTokenRepository;
            _accessMenuRepository = accessMenuRepository;
            _userRoleReository = userRoleReository;
            _view_UserMenubarRepository = view_UserMenubarRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        public async Task<IActionResult> UsersList(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersNoTrackingAsync(cancellationToken);

            List<RegisterDto> newList = users.Select(user => new RegisterDto
            {
                UserId = user.Id,
                FirstName = user.UserProfiles.Select(p => p.FirstName).FirstOrDefault(),
                LastName = user.UserProfiles.Select(p => p.LastName).FirstOrDefault(),
                NationalCode = user.UserProfiles.Select(p => p.NationalCode).FirstOrDefault(),
                UserName = user.UserName,
                Password = string.Empty,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                UserTypeTitle = user.UserRoles.Select(p => p.Role.RoleNameFa).Last(),
                RoleId = user.UserRoles.Select(p => p.RoleId).Last(),
                IsActive = user.IsActive,
                IsActiveTitle = (IsActiveTitleType)(user.IsActive ? 1 : 0),
                IsDelete = user.IsDeleted

            }).ToList();

            return PartialView("UsersList",newList);
        }

        public async Task<IActionResult> DetailUser(Guid userid, CancellationToken cancellationToken)
        {
            try
            {
                if (userid == Guid.Empty)
                {
                    return NotFound();
                }
                var user = await _userRepository.GetByGuidIdAsync(userid, cancellationToken);
                //var user1 = await _userRepository.GetByGuidIdAsync(userid,cancellationToken);
                if (user == null)
                {
                    return NotFound();
                }
                var registerDto = new RegisterDto
                {
                    UserId = user.Id,
                    FirstName = user.UserProfiles.Any() ? user.UserProfiles.Select(p => p.FirstName).FirstOrDefault() : null,
                    LastName = user.UserProfiles.Any() ? user.UserProfiles.Select(p => p.LastName).FirstOrDefault() : null,
                    NationalCode = user.UserProfiles.Any() ? user.UserProfiles.Select(p => p.NationalCode).FirstOrDefault() : null,
                    UserTypeTitle = user.UserRoles.Any() ? user.UserRoles.Select(p => p.Role.RoleNameFa).Last() : null,
                    RoleId = user.UserRoles.Any() ? user.UserRoles.Select(p => p.RoleId).Last() : 0,
                    UserName = user.UserName,
                    Password = string.Empty,
                    EmailAddress = user.EmailAddress,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    IsActiveTitle = (IsActiveTitleType)(user.IsActive ? 1 : 0),
                    IsDelete = user.IsDeleted

                };
                return PartialView("DetailUser", registerDto);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region Add User
        public IActionResult CreateUser()
        {
            var roles = _eFRepositoryRole.TableNoTracking.ToList();
            var model = new RegisterDto();
            roles.Insert(0, new Role
            {
                Id = 0,
                RoleNameFa = "..."
            });
            ViewBag.ListofRoles = roles;

            return PartialView("CreateUser", model); // تغییر به PartialView
        }
        [HttpPost]
        public async Task<ApiResult> CreateUser([Bind("FirstName,LastName,NationalCode,UserName,Password,EmailAddress,PhoneNumber,RoleId,IsActive")] RegisterDto registertDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            var curentUserToken = User.Identity.FindFirstValue("Token");
            var token = await _userTokenRepository.GetUserTokenByIdAsync(new Guid(curentUserToken), cancellationToken);
            registertDto = await CheckValidation(registertDto, token.UserId, cancellationToken);
            if (!ModelState.IsValid)
            {
                var role = _eFRepositoryRole.TableNoTracking.ToList();
                role.Insert(0, new Role
                {
                    Id = 0,
                    RoleNameFa = "..."
                });
                ViewBag.ListofRoles = role;
                try
                {
                    var validationSummary = string.Join("<br>", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));

                    return new BadRequestObjectResult(ModelState);

                }
                catch (Exception ex)
                {

                    throw;
                }


            }

            User user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = registertDto.UserName,
                NormalizedUserName = registertDto.UserName.FixTextUpper(),
                EmailAddress = registertDto.EmailAddress,
                NormalizedEmailAddress = registertDto.EmailAddress.FixTextUpper(),
                EmailAddressConfirmed = false,
                PasswordHash = PasswordHelper.HashPasswordBCrypt(registertDto.Password),
                ConcurrencyStamp = Guid.NewGuid(),
                PhoneNumber = registertDto.PhoneNumber,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                CreatedBy = token.UserId,
                HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            var userProfile = _mapper.Map<UserProfile>(registertDto);
            userProfile.UserId = user.Id;
            userProfile.CreatedBy = user.CreatedBy;

            UserRole userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = registertDto.RoleId,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                CreatedBy = user.CreatedBy,
                HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            await _userRepository.AddAsync(user, cancellationToken, false);
            await _userProfilesRepository.AddAsync(userProfile, cancellationToken, false);
            await _userRoleReository.AddAsync(userRole, cancellationToken, false);

            await _userRepository.SaveChangesAsync(cancellationToken);
            await _userProfilesRepository.SaveChangesAsync(cancellationToken);
            await _userRoleReository.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        #endregion End Add User

        #region Edit User
        public async Task<IActionResult> EditUser(Guid userid, CancellationToken cancellationToken)
        {
            try
            {
                var roles = _eFRepositoryRole.TableNoTracking.ToList();
                var model = new RegisterDto();
                roles.Insert(0, new Role
                {
                    Id = 0,
                    RoleNameFa = "..."
                });
                ViewBag.ListofRoles = roles;

                if (userid == null)
                {
                    return NotFound();
                }
                var user = await _userRepository.GetByGuidIdAsync(userid, cancellationToken);
                if (user == null)
                {
                    return NotFound();
                }
                var registerDto = new RegisterDto
                {

                    UserId = user.Id,
                    FirstName = user.UserProfiles.Select(p => p.FirstName).FirstOrDefault(),
                    LastName = user.UserProfiles.Select(p => p.LastName).FirstOrDefault(),
                    NationalCode = user.UserProfiles.Select(p => p.NationalCode).FirstOrDefault(),
                    UserName = user.UserName,
                    Password = string.Empty,
                    EmailAddress = user.EmailAddress,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.UserRoles.Select(p => p.RoleId).Last(),
                    IsActive = user.IsActive,
                };

                return PartialView("EditUser", registerDto);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<ApiResult> EditUser([Bind("UserId,FirstName,LastName,NationalCode,UserName,Password,EmailAddress,PhoneNumber,RoleId,IsActive")] RegisterDto registertDto, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(registertDto.Password))
                {
                    ModelState.Remove(nameof(registertDto.Password));
                }

                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }

                var user = await _userRepository.Table.AsNoTracking().FirstOrDefaultAsync(u => u.Id == registertDto.UserId, cancellationToken);
                var userProfile = await _userProfilesRepository.Table.AsNoTracking().FirstOrDefaultAsync(up => up.UserId == registertDto.UserId, cancellationToken);
                var userRole = await _userRoleReository.Table.AsNoTracking().FirstOrDefaultAsync(ur => ur.UserId == registertDto.UserId, cancellationToken);

                var curentUserToken = User.Identity.FindFirstValue("Token");
                var token = await _userTokenRepository.GetUserTokenByIdAsync(new Guid(curentUserToken), cancellationToken);

                if (user.NormalizedUserName != registertDto.UserName.FixTextUpper())
                {
                    var userExist = await _userRepository.Table.AsNoTracking().FirstOrDefaultAsync(u => u.NormalizedUserName == registertDto.UserName.FixTextUpper(), cancellationToken);
                    if (userExist != null)
                    {
                        ModelState.AddModelError("UserName", "نام کاربری تکراری است");
                    }
                }
                if (user.NormalizedEmailAddress != registertDto.EmailAddress.FixTextUpper())
                {
                    var userExist = await _userRepository.Table.AsNoTracking().FirstOrDefaultAsync(u => u.NormalizedEmailAddress == registertDto.EmailAddress.FixTextUpper(), cancellationToken);
                    if (userExist != null)
                    {
                        ModelState.AddModelError("EmailAddress", "ایمیل تکراری است");
                    }
                }
                if (user.PhoneNumber != registertDto.PhoneNumber)
                {
                    var userExist = await _userRepository.Table.AsNoTracking().FirstOrDefaultAsync(u => u.PhoneNumber == registertDto.PhoneNumber, cancellationToken);
                    if (userExist != null)
                    {
                        ModelState.AddModelError("PhoneNumber", "شماره همراه تکراری است");
                    }
                }
                if (userProfile.NationalCode != registertDto.NationalCode)
                {
                    var userExist = await _userProfilesRepository.Table.AsNoTracking().FirstOrDefaultAsync(up => up.NationalCode == registertDto.NationalCode.FixTextUpper(), cancellationToken);
                    if (userExist != null)
                    {
                        ModelState.AddModelError("PhoneNumber", "کدملی تکراری است");
                    }
                }

                if (!ModelState.IsValid)
                {
                    var role = _eFRepositoryRole.TableNoTracking.ToList();
                    role.Insert(0, new Role
                    {
                        Id = 0,
                        RoleNameFa = "..."
                    });
                    ViewBag.ListofRoles = role;
                    return new BadRequestObjectResult(ModelState);
                }

                user.UserName = registertDto.UserName;
                user.NormalizedUserName = registertDto.UserName.FixTextUpper();
                user.EmailAddress = registertDto.EmailAddress;
                user.NormalizedEmailAddress = registertDto.EmailAddress.FixTextUpper();
                user.EmailAddressConfirmed = false;
                user.ConcurrencyStamp = Guid.NewGuid();
                user.PhoneNumber = registertDto.PhoneNumber;
                user.PhoneNumberConfirmed = false;
                user.TwoFactorEnabled = false;
                user.LockoutEnd = null;
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                user.CreatedBy = token.UserId;
                user.IsActive = registertDto.IsActive;
                user.HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                if (!string.IsNullOrEmpty(registertDto.Password))
                {
                    user.PasswordHash = PasswordHelper.HashPasswordBCrypt(registertDto.Password);
                }

                userProfile.FirstName = registertDto.FirstName;
                userProfile.LastName = registertDto.LastName;
                userProfile.NationalCode = registertDto.NationalCode;
                userProfile.IsActive = registertDto.IsActive;
                userProfile.HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                userRole.RoleId = registertDto.RoleId;

                _userRepository.Attach(user);
                _userProfilesRepository.Attach(userProfile);
                _userRoleReository.Attach(userRole);

                _userRepository.Update(user);
                _userProfilesRepository.Update(userProfile);
                _userRoleReository.Update(userRole);

                await _userRepository.SaveChangesAsync(cancellationToken);
                await _userProfilesRepository.SaveChangesAsync(cancellationToken);
                await _userRoleReository.SaveChangesAsync(cancellationToken);

                // تست ثبت لاگ کاربر
                //var oldUser = dbContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == user.Id);

                //// Update user
                //dbContext.Users.Update(user);
                //dbContext.SaveChanges();

                //// Create AuditLog entry
                //var auditLog = new AuditLog
                //{
                //    AuditLogId = Guid.NewGuid(),
                //    EntityId = user.Id,
                //    TableName = "User",
                //    OperationType = "Update",
                //    OperationTime = DateTime.UtcNow,
                //    UserId = user.CreatorID,
                //    OldValue = JsonConvert.SerializeObject(oldUser),
                //    NewValue = JsonConvert.SerializeObject(user),
                //    Description = "User updated"
                //};
                //dbContext.AuditLogs.Add(auditLog);
                //dbContext.SaveChanges();


                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
        #endregion End Edit User

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid userid, CancellationToken cancellationToken)
        {
            try
            {


                if (userid == null)
                {
                    return Json(new { isSuccess = false, message = "کاربری جهت حذف ارسال نشده است یا خطایی رخ داده." });
                }
                var curentUserToken = User.Identity.FindFirstValue("Token");
                var token = await _userTokenRepository.GetUserTokenByIdAsync(new Guid(curentUserToken), cancellationToken);
                if (token != null)
                {
                    if (userid == token.UserId)
                    {
                        return Json(new { isSuccess = false, message = "امکان حذف کاربری جاری وجود ندارد." });
                    }
                }

                var user = await _userRepository.GetByIdAsync(cancellationToken, userid);
                if (user == null)
                {
                    return Json(new { isSuccess = false, message = "کاربری یافت نشد." });
                }
                user.IsDeleted = true;

                await _userRepository.UpdateAsync(user, cancellationToken);

                return Json(new { isSuccess = true, message = "کاربر با موفقیت حذف شد." });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region بررسی و صحت سنجی
        private async Task<RegisterDto> CheckValidation(RegisterDto registerDto, Guid CurentUserID, CancellationToken cancellationToken)
        {
            if (registerDto.RoleId == 0)
            {
                ModelState.AddModelError("RoleId", "نوع کاربری را انتخاب کنید");
            }
            var ExistEamil = await _userRepository.GetUserByEmailAsync(registerDto.EmailAddress.FixTextUpper(), cancellationToken);
            if (ExistEamil != null)
            {
                ModelState.AddModelError("EmailAddress", " Email تکراری است");
            }
            var ExistUserName = await _userRepository.GetUserByUserNameAsync(registerDto.UserName.FixTextUpper(), cancellationToken);
            if (ExistUserName != null)
            {
                ModelState.AddModelError("UserName", "نام کاربری تکراری است");
            }
            var ExistPhoneNumber = await _userRepository.GetUserByPhoneNumberAsync(registerDto.PhoneNumber.FixTextUpper(), cancellationToken);
            if (ExistPhoneNumber != null)
            {
                ModelState.AddModelError("PhoneNumber", " شماره همراه تکراری است");
            }
            var ExistNationalCode = await _userProfilesRepository.GetProfileByNationalCodeAsync(registerDto.NationalCode.FixTextUpper(), cancellationToken);
            if (ExistNationalCode != null)
            {
                ModelState.AddModelError("NationalCode", " کد ملی تکراری است");
            }
            return registerDto;
        }
        #endregion

        #region دریافت فهرست منو بر اساس سطح دسترسی کاربر و نمای سطح دسترسی کاربر انتخابی
        public IActionResult UserAccessMenu(Guid userid)
        {
            ViewData["userId"] = userid;
            return PartialView("UserAccessMenu");
        }
        public async Task<ActionResult<string>> GetUserAccessMenuItem(Guid userid, CancellationToken cancellationToken)
        {
            #region Convert ListMenu to ListMenuUserAccessDto for use jstree
            List<Menu> listMenus = await _menuRepository.GetAllMenusAsync(cancellationToken);
            List<ListMenuUserAccessDto> listMenuUserAccessDtos = new List<ListMenuUserAccessDto>();
            foreach (var item in listMenus)
            {
                if (item.ParentId == null)
                {
                    listMenuUserAccessDtos.Add(new ListMenuUserAccessDto
                    {
                        id = item.Id,
                        parent = "#",
                        text = item.MenuTitle,
                    });
                }
                else
                {
                    listMenuUserAccessDtos.Add(new ListMenuUserAccessDto
                    {
                        id = item.Id,
                        parent = item.ParentId.ToString(),
                        text = item.MenuTitle,
                    });
                }
            }
            #endregion

            #region Add User Access item
            List<View_UserMenubar> view_UserMenubars = _view_UserMenubarRepository.GetUserAccessMenus(userid, cancellationToken);
            foreach (var item in view_UserMenubars)
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
                        listMenuUserAccessStateDto.disabled = item.Group_Checkecd;
                        item.state = listMenuUserAccessStateDto;
                        if (item.Group_Checkecd == true)
                        {
                            JStreeAttr jStreeAttr = new JStreeAttr()
                            {
                                href = "test1",
                                title = "TitleTest"
                            };
                            item.a_attr = jStreeAttr;
                        }
                    }
                    else
                    {
                        ListMenuUserAccessStateDto listMenuUserAccessStateDto = new ListMenuUserAccessStateDto();
                        listMenuUserAccessStateDto.selected = false;
                        listMenuUserAccessStateDto.opened = false;
                        listMenuUserAccessStateDto.disabled = item.Group_Checkecd;
                        item.state = listMenuUserAccessStateDto;
                        if (item.Group_Checkecd == true)
                        {
                            JStreeAttr jStreeAttr = new JStreeAttr()
                            {
                                href = "test1",
                                title = "TitleTest"
                            };
                            item.a_attr = jStreeAttr;
                        }
                    }
                }
            }
            #endregion

            var json = JsonConvert.SerializeObject(listMenuUserAccessDtos);
            return (json);
        }
        #endregion پایان

        #region افزودن و اصلاح سطح دسترسی کاربر انتخابی
        public async Task<IActionResult> Modify_SelectedNodes_SelectedUser(string list, Guid userId, CancellationToken cancellationToken)
        {
            List<UserAccessMenus> userAccessMenus = await _accessMenuRepository.GetUserAccessMenusByUserIdAsync(userId, cancellationToken);
            //Remove all access
            if (list is null)
            {
                foreach (var item in userAccessMenus)
                {
                    item.IsDeleted = true;
                    await _accessMenuRepository.UpdateAsync(item, cancellationToken);
                }
            }
            else
            {
                string[] strArray = list.Split(",");
                foreach (var item in strArray)
                {
                    bool isExist = userAccessMenus.Any(ua => ua.MenuId.Equals(new Guid(item)));
                    if (!isExist)
                    {
                        UserAccessMenus NewAccess = new UserAccessMenus();
                        NewAccess.MenuId = new Guid(item);
                        NewAccess.UserId = userId;
                        NewAccess.CreatedBy = new Guid(User.Identity.FindFirstValue(ClaimTypes.NameIdentifier));
                        NewAccess.IsDeleted = false;
                        NewAccess.HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        await _accessMenuRepository.AddAsync(NewAccess, cancellationToken);
                    }
                }
                foreach (var item in userAccessMenus)
                {
                    bool isExist = strArray.Any(ua => ua.Equals(item.MenuId.ToString()));
                    if (!isExist)
                    {
                        item.IsDeleted = true;
                        await _accessMenuRepository.UpdateAsync(item, cancellationToken);
                    }
                    else if (isExist && item.IsDeleted == true)
                    {
                        item.IsDeleted = false;
                        await _accessMenuRepository.UpdateAsync(item, cancellationToken);
                    }
                }
            }
            return Json(new { isSuccess = true, message = "اصلاح دسترسی با موفقیت انجام شد." });
        }
        #endregion پایان

    }
}