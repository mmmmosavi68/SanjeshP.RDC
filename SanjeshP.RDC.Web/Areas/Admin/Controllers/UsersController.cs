using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Convertor;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Data.Repositories;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User;
using System;
using System.Collections.Generic;
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
        private readonly IView_UserMenubarRepository _view_UserMenubarRepository;

        public UsersController(IMapper mapper
                                , ICompositeViewEngine viewEngine
                                , ILogger<UsersController> logger
                                , IUserRepository userRepository
                                , IUserProfilesRepository userProfilesRepository
                                , IEFRepository<Role> eFRepositoryRole
                                , IMenuRepository menuRepository
                                , IView_UserMenubarRepository view_UserMenubarRepository
                                , IUserTokenRepository userTokenRepository)
        {
            _mapper = mapper;
            _viewEngine = viewEngine;
            _logger = logger;
            _userRepository = userRepository;
            _userProfilesRepository = userProfilesRepository;
            _eFRepositoryRole = eFRepositoryRole;
            _menuRepository = menuRepository;
            _userTokenRepository = userTokenRepository;
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


        public IActionResult AddUser()
        {
            return PartialView("AddUser");
        }
        public async Task<IActionResult> AddUser([Bind("FirstName,LastName,NationalCode,UserName,Password,EmailAddress,PhoneNumber,RoleId,IsActive")] RegisterDto registertDto, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(registertDto);
                await _userRepository.AddAsync(user, cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("AddUser", registertDto);
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

       public IActionResult UserAccessMenu(Guid userid)
        {
            return ViewComponent("UserAccessMenu", new { userid = userid });
        }

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
