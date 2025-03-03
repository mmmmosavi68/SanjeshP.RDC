using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Convertor;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Security;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User;
using SanjeshP.RDC.Web.Models.Identity;
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
        private readonly IUserProfilesRepository _userProfilesRepository;
        private readonly IEFRepository<Role> _eFRepositoryRole;
        private readonly IMenuRepository _menuRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IAccessMenuRepository _accessMenuRepository;
        private readonly IEFRepository<UserRole> _eFRepositoryUserRole;
        private readonly IView_UserMenubarRepository _view_UserMenubarRepository;

        public UsersController(IMapper mapper
                                , ICompositeViewEngine viewEngine
                                , ILogger<UsersController> logger
                                , IUserRepository userRepository
                                , IUserProfilesRepository userProfilesRepository
                                , IEFRepository<Role> eFRepositoryRole
                                , IMenuRepository menuRepository
                                , IView_UserMenubarRepository view_UserMenubarRepository
                                , IUserTokenRepository userTokenRepository
                                , IAccessMenuRepository accessMenuRepository
                                , IEFRepository<UserRole> eFRepositoryUserRole)
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
            _eFRepositoryUserRole = eFRepositoryUserRole;
            _view_UserMenubarRepository = view_UserMenubarRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByAllNoTrackingAsync(cancellationToken);

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
                UserTypeTitle = user.UserRoles.Select(p => p.Role.RoleTitleFa).Last(),
                RoleId = user.UserRoles.Select(p => p.RoleId).Last(),
                IsActive = user.IsActive,
                IsActiveTitle = (IsActiveTitleType)(user.IsActive ? 1 : 0),
                IsDelete = user.IsDelete

            }).ToList();

            return View(newList);
        }

        public async Task<IActionResult> DetailUser(Guid userid, CancellationToken cancellationToken)
        {
            if (userid == Guid.Empty)
            {
                return NotFound();
            }
            var user = await _userRepository.GetByIdAsync(userid, cancellationToken);
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
                UserTypeTitle = user.UserRoles.Select(p => p.Role.RoleTitleFa).Last(),
                RoleId = user.UserRoles.Select(p => p.RoleId).Last(),
                IsActive = user.IsActive,
                IsActiveTitle = (IsActiveTitleType)(user.IsActive ? 1 : 0),
                IsDelete = user.IsDelete

            };
            return PartialView("DetailUser", registerDto);
        }

        public IActionResult CreateUser()
        {
            var roles = _eFRepositoryRole.TableNoTracking.ToList();
            var model = new RegisterDto();
            roles.Insert(0, new Role
            {
                Id = 0,
                RoleTitleFa = "..."
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
            var token = await _userTokenRepository.GetByIdAsync(new Guid(curentUserToken), cancellationToken);
            registertDto = await CheckValidation(registertDto, token.UserId, cancellationToken);
            if (!ModelState.IsValid)
            {
                var role = _eFRepositoryRole.TableNoTracking.ToList();
                role.Insert(0, new Role
                {
                    Id = 0,
                    RoleTitleFa = "..."
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
                Creator = token.UserId,
                HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            var userProfile = _mapper.Map<UserProfile>(registertDto);
            userProfile.UserId = user.Id;
            userProfile.Creator = user.Creator;

            UserRole userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = registertDto.RoleId,
                IsActive = true,
                IsDelete = false,
                CreateDate = DateTime.Now,
                Creator = user.Creator,
                HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            await _userRepository.AddAsync(user, cancellationToken, false);
            await _userProfilesRepository.AddAsync(userProfile, cancellationToken, false);
            await _eFRepositoryUserRole.AddAsync(userRole, cancellationToken, false);

            await _userRepository.SaveChangesAsync(cancellationToken);
            await _userProfilesRepository.SaveChangesAsync(cancellationToken);
            await _eFRepositoryUserRole.SaveChangesAsync(cancellationToken);

            return Ok();
        }


        public async Task<IActionResult> EditUser(Guid userid, CancellationToken cancellationToken)
        {
            var roles = _eFRepositoryRole.TableNoTracking;
            ViewBag.ListofRoles = roles;

            if (userid == null)
            {
                return NotFound();
            }
            var user = await _userRepository.GetByIdAsync(userid, cancellationToken);
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
        [HttpPost]
        public async Task<IActionResult> EditUser([Bind("UserId,FirstName,LastName,NationalCode,UserName,Password,EmailAddress,PhoneNumber,RoleId,IsActive")] RegisterDto registertDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(registertDto.Password))
            {
                ModelState.Remove(nameof(registertDto.Password));
            }
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetById(registertDto.UserId);
                var userProfile = _userProfilesRepository.GetByUserIdAsync(registertDto.UserId, cancellationToken);

                if (user.NormalizedUserName != registertDto.UserName.FixTextUpper())
                {
                    var userExist = await _userRepository.GetByUserNameAsync(registertDto.UserName.FixTextUpper(), cancellationToken);
                    if (userExist != null)
                    {
                        ModelState.AddModelError("UserName", "نام کاربری تکراری است");
                        var role = _eFRepositoryRole.TableNoTracking;
                        ViewBag.ListofRoles = role;
                        return PartialView("EditUser", registertDto);
                    }
                }
                else if (user.NormalizedEmailAddress != registertDto.EmailAddress.FixTextUpper())
                {
                    var userExist = await _userRepository.GetByEmailAsync(registertDto.EmailAddress.FixTextUpper(), cancellationToken);
                    if (userExist != null)
                    {
                        ModelState.AddModelError("EmailAddress", "ایمیل تکراری است");
                        var role = _eFRepositoryRole.TableNoTracking;
                        ViewBag.ListofRoles = role;
                        return Json(new { isSuccess = true, html = RenderRazorViewToString("EditUser.cshtml", registertDto) });

                        //return PartialView("EditUser", registerEditDto);
                    }
                }
                else if (user.PhoneNumber != registertDto.PhoneNumber.FixTextUpper())
                {
                    var userExist = await _userRepository.GetByPhoneNumberAsync(registertDto.PhoneNumber, cancellationToken);
                    if (userExist != null)
                    {
                        ModelState.AddModelError("PhoneNumber", "شماره همراه تکراری است");
                        var role = _eFRepositoryRole.TableNoTracking;
                        ViewBag.ListofRoles = role;
                        return RedirectToAction("Index", registertDto);
                    }
                }

                // در صورت موفقیت، کاربر را ویرایش کنید و به صفحه اصلی بازگردید
                // کد ویرایش کاربر
                return Json(new { isSuccess = true });
            }

            // در صورت وجود خطا، PartialView را با خطاها بازگردانید
            var roles = _eFRepositoryRole.TableNoTracking;
            ViewBag.ListofRoles = roles;
            return PartialView("EditUser", registertDto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid userid, CancellationToken cancellationToken)
        {
            if (userid == null)
            {
                return Json(new { isSuccess = false, message = "کاربری جهت حذف ارسال نشده است یا خطایی رخ داده." });
            }
            var curentUserToken = User.Identity.FindFirstValue("Token");
            var token = await _userTokenRepository.GetByIdAsync(new Guid(curentUserToken), cancellationToken);
            if (token != null)
            {
                if (userid == token.UserId)
                {
                    return Json(new { isSuccess = false, message = "امکان حذف کاربری جاری وجود ندارد." });
                }
            }

            var user = await _userRepository.GetByIdAsync(userid, cancellationToken);
            if (user == null)
            {
                return Json(new { isSuccess = false, message = "کاربری یافت نشد." });
            }

            await _userRepository.DeleteAsync(user, cancellationToken);

            return Json(new { isSuccess = true, message = "کاربر با موفقیت حذف شد." });
        }

        #region دریافت فهرست منو بر اساس سطح دسترسی کاربر و نمای سطح دسترسی کاربر انتخابی
        public IActionResult UserAccessMenu(Guid userid)
        {
            ViewData["userId"] = userid;
            return PartialView("UserAccessMenu");
        }
        public async Task<ActionResult<string>> GetUserAccessMenuItem(Guid userid, CancellationToken cancellationToken)
        {
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

            #region Add User Access item
            List<View_UserMenubar> view_UserMenubars = _view_UserMenubarRepository.GetUserAccessMenu(userid, cancellationToken);
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
            List<AccessMenus> userAccessMenus = await _accessMenuRepository.GetAllByUserIdAsync(userId, cancellationToken);
            //Remove all access
            if (list is null)
            {
                foreach (var item in userAccessMenus)
                {
                    item.IsDelete = true;
                    await _accessMenuRepository.UpdateAsync(item, cancellationToken);
                }
            }
            else
            {
                string[] strArray = list.Split(",");
                foreach (var item in strArray)
                {
                    bool isExist = userAccessMenus.Any(ua => ua.ListMenuId.Equals(new Guid(item)));
                    if (!isExist)
                    {
                        AccessMenus NewAccess = new AccessMenus();
                        NewAccess.ListMenuId = new Guid(item);
                        NewAccess.UserId = userId;
                        NewAccess.Creator = new Guid(User.Identity.FindFirstValue(ClaimTypes.NameIdentifier));
                        NewAccess.IsDelete = false;
                        NewAccess.HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        await _accessMenuRepository.AddAsync(NewAccess, cancellationToken);
                    }
                }
                foreach (var item in userAccessMenus)
                {
                    bool isExist = strArray.Any(ua => ua.Equals(item.ListMenuId.ToString()));
                    if (!isExist)
                    {
                        item.IsDelete = true;
                        await _accessMenuRepository.UpdateAsync(item, cancellationToken);
                    }
                    else if (isExist && item.IsDelete == true)
                    {
                        item.IsDelete = false;
                        await _accessMenuRepository.UpdateAsync(item, cancellationToken);
                    }
                }
            }
            return Json(new { isSuccess = true, message = "اصلاح دسترسی با موفقیت انجام شد." });
        }
        #endregion پایان


        #region بررسی و صحت سنجی
        private async Task<RegisterDto> CheckValidation(RegisterDto registerDto, Guid CurentUserID, CancellationToken cancellationToken)
        {
            if (registerDto.RoleId == 0)
            {
                ModelState.AddModelError("RoleId", "نوع کاربری را انتخاب کنید");
            }
            var ExistEamil = await _userRepository.GetByEmailAsync(registerDto.EmailAddress.FixTextUpper(), cancellationToken);
            if (ExistEamil != null)
            {
                ModelState.AddModelError("EmailAddress", " Email تکراری است");
            }
            var ExistUserName = await _userRepository.GetByUserNameAsync(registerDto.UserName.FixTextUpper(), cancellationToken);
            if (ExistUserName != null)
            {
                ModelState.AddModelError("UserName", "نام کاربری تکراری است");
            }
            var ExistPhoneNumber = await _userRepository.GetByPhoneNumberAsync(registerDto.PhoneNumber.FixTextUpper(), cancellationToken);
            if (ExistPhoneNumber != null)
            {
                ModelState.AddModelError("PhoneNumber", " شماره همراه تکراری است");
            }
            var ExistNationalCode = await _userProfilesRepository.GetByCodeMeliAsync(registerDto.NationalCode.FixTextUpper(), cancellationToken);
            if (ExistNationalCode != null)
            {
                ModelState.AddModelError("NationalCode", " کد ملی تکراری است");
            }
            return registerDto;
        }
        #endregion
        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.GetView("/Areas/Admin/Views/Users/", viewName, false);
                if (viewResult.View == null)
                {

                    throw new ArgumentNullException($"View '{viewName}' not found.");
                }

                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw, new HtmlHelperOptions());
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }


    }
}