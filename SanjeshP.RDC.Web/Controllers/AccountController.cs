using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SanjeshP.RDC.Entities.User;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using SanjeshP.RDC.Data.Contracts.Users;
using SanjeshP.RDC.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SanjeshP.RDC.Web.SharedViewModels;
using System.Collections.Generic;
using System;
using System.Linq;
using SanjeshP.RDC.Common;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using SanjeshP.RDC.Data.Contracts.Menus;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pipelines.Sockets.Unofficial.Arenas;

namespace SanjeshP.RDC.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IViewUserMenubarRepository _viewUserMenubarRepository;
        private readonly SiteSettings _siteSetting;
        public AccountController(IMapper mapper
                                , IUserRepository userRepository
                                , ILogger<AccountController> logger
                                , IUserTokenRepository userTokenRepository
                                , IOptionsSnapshot<SiteSettings> siteSetting
                                , IMemoryCache memoryCache
                                , IViewUserMenubarRepository viewUserMenubarRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
            _userTokenRepository = userTokenRepository;
            _memoryCache = memoryCache;
            _viewUserMenubarRepository = viewUserMenubarRepository;
            _siteSetting = siteSetting.Value;
        }


        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            // جستجوی کاربر بر اساس نام کاربری
            var user = await _userRepository.GetUserByUserNameAsync(loginViewModel.UserName, cancellationToken);

            // اعتبارسنجی وجود کاربر
            if (user == null)
            {
                ModelState.AddModelError("UserName", "کاربری با چنین مشخصاتی یافت نشد");
                _logger.LogWarning($"Login failed: {loginViewModel.UserName} not found.");
                return View(loginViewModel);
            }

            // بررسی رمز عبور
            if (!PasswordHelper.VerifyPasswordBCrypt(loginViewModel.Password, user.PasswordHash))
            {
                ModelState.AddModelError("Password", "نام کاربری یا رمز عبور اشتباه است");
                _logger.LogWarning($"Login failed: password verification failed for user {user.UserName}.");
                return View(loginViewModel);
            }

            // بررسی حساب حذف‌شده
            if (user.IsDeleted)
            {
                ModelState.AddModelError("UserName", "حساب کاربری شما حذف شده است");
                _logger.LogWarning($"Login failed: user {user.UserName} is deleted.");
                return View(loginViewModel);
            }

            // بررسی فعال‌بودن حساب
            if (!user.IsActive)
            {
                ModelState.AddModelError("UserName", "حساب کاربری شما فعال نشده است");
                _logger.LogWarning($"Login failed: user {user.UserName} is not active.");
                return View(loginViewModel);
            }

            // بررسی انقضای حساب کاربری
            if (user.ExpireDate < DateTime.Now)
            {
                ModelState.AddModelError("UserName", "حساب کاربری شما منقضی شده است");
                _logger.LogWarning($"Login failed: user {user.UserName} account is expired.");
                return View(loginViewModel);
            }

            // حذف تمام توکن های قبلی کاربر - در صورت لاگین هممزان باید اصلاح شود
            await _userTokenRepository.MarkExpiredTokensAsDeletedAsync(user.Id, cancellationToken);


            // ساخت توکن و ثبت اطلاعات
            var token = new UserToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                SessionId = HttpContext.Session.Id,
                UserAgent = Request.Headers["User-Agent"].ToString(),
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMinutes(_siteSetting.authenticationSettings.ExpireTimeSpanInMinutes),
                HostIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
            };

            await _userTokenRepository.AddUserTokenAsync(token, cancellationToken);

            // تعریف Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserProfiles.Any() ? $"{user.UserProfiles.First().FirstName} {user.UserProfiles.First().LastName}" : "No Profile"),
                new Claim("RoleID", user.UserRoles.Any() ? user.UserRoles.Last().Role.NormalizedRoleNameEn : "No Role"),
                new Claim("RoleNameFa", user.UserRoles.Any() ? user.UserRoles.First().Role.RoleNameFa : "No Role"),
                new Claim("Token", token.Id.ToString())
            };


            #region ذخیره سطح دسترسی کاربر در memoryCache
            var userPermissions = _viewUserMenubarRepository.GetUserAccessMenus(user.Id, cancellationToken);
            var cacheKey = $"UserPermissions_{token.Id}";
            var accessData = userPermissions.Select(x => new UserAccessViewModel
            {
                MenuTitle = x.MenuTitle ?? "Default Title", // مقدار پیش‌فرض برای null
                PageCode = x.PageCode.HasValue ? (int)x.PageCode : 0, // استفاده از مقدار پیش‌فرض
                SowMenu = x.ShowMenu ?? false, // مقدار پیش‌فرض برای bool
                CssClass = x.CssClass ?? "default-class",
                Icon = x.Icon ?? "default-icon",
                Area = x.Area ?? "default-area",
                ControllerName = x.ControllerName ?? "default-controller",
                ActionName = x.ActionName ?? "default-action",
                Person_Checkecd = x.Person_Checkecd,
                Group_Checkecd = x.Group_Checkecd,
                Disabled = x.disabled,
                IsParent = x.IsParent,
                FirstName = user.UserProfiles.FirstOrDefault()?.FirstName ?? "Default FirstName",
                LastName = user.UserProfiles.FirstOrDefault()?.LastName ?? "Default LastName",
                RoleNameFa = user.UserRoles.FirstOrDefault()?.Role?.RoleNameFa ?? "Default Role"
            }).ToList();

            _memoryCache.Set(cacheKey, accessData, TimeSpan.FromMinutes(30));
            #endregion

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = loginViewModel.RememberMe // برای لاگین ماندگار
            };

            // ثبت ورود
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            _logger.LogInformation($"Login succeeded: user {user.UserName} authenticated successfully.");
            return Redirect("/Admin/Home");
        }
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = _mapper.Map<User>(registerViewModel);
            user.PasswordHash = PasswordHelper.HashPasswordBCrypt(registerViewModel.Password);
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;

            await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation($"User {registerViewModel.UserName} registered successfully.");

            return RedirectToAction("Login");
        }
        #endregion

        #region Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login");
        }
        #endregion
    }
}