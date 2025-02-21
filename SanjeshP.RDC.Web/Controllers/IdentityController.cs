using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SanjeshP.RDC.Entities.User;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using System;
using SanjeshP.RDC.Web.Models.Identity;
using SanjeshP.RDC.Data.Contracts;
using System.Linq;
using AutoMapper;
using SanjeshP.RDC.Security;
using SanjeshP.RDC.Convertor;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Data.Repositories;
using SanjeshP.RDC.Models.DTO_Menu;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Common;
using Microsoft.Extensions.Logging;

namespace SanjeshP.RDC.Web.Controllers
{
    [AllowAnonymous]
    public class IdentityController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IdentityController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IView_UserMenubarRepository _view_UserMenubarRepository;
        private readonly IEFRepository<UserRole> _eFUserRoleRepository;
        private readonly IEFRepository<UserProfile> _userProfilesRepository;

        public IdentityController(IMapper mapper
                                 , ILogger<IdentityController> logger
                                 , IUserRepository userRepository
                                 , IUserTokenRepository userTokenRepository
                                 , IView_UserMenubarRepository view_UserMenubarRepository
                                 , IEFRepository<UserRole> eFUserRoleRepository
                                 , IEFRepository<UserProfile> userProfilesRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _view_UserMenubarRepository = view_UserMenubarRepository;
            _eFUserRoleRepository = eFUserRoleRepository;
            _userProfilesRepository = userProfilesRepository;
        }
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                //string PasswordHash = PasswordHelper.HashPasswordBCrypt(loginDTO.Password);
                //User users = await _userRepository.GetByUserNameAndPasswordAsync(loginDTO.UserName, PasswordHash, cancellationToken);
                User user = await _userRepository.GetByUserNameAsync(loginDTO.UserName, cancellationToken);

                if (user == null)
                {
                    ModelState.AddModelError("UserName", "کاربری با چنین مشخصاتی یافت نشد");
                    _logger.LogInformation("Login failed:" + loginDTO.UserName + " Not Found.");
                    return View(loginDTO);
                }
                if (!PasswordHelper.VerifyPasswordBCrypt(loginDTO.Password, user.PasswordHash))
                {
                    ModelState.AddModelError("UserName", "کاربری با چنین مشخصاتی یافت نشد");
                    _logger.LogInformation("Login failed:" + user.UserName + " Password verification failed.");
                    return View(loginDTO);
                }
                else if (user.IsDelete)
                {
                    ModelState.AddModelError("UserName", "کاربری با چنین مشخصاتی یافت نشد");
                    _logger.LogInformation("Login failed: " + user.UserName + " Deleted");
                    return View(loginDTO);
                }
                else if (!user.IsActive)
                {
                    ModelState.AddModelError("UserName", "حساب کاربری شما فعال نشده است");
                    _logger.LogInformation("Login failed: " + user.UserName + " Not Active.");
                    return View(loginDTO);
                }
                else if (user.ExpireDate < DateTime.Now)
                {
                    ModelState.AddModelError("UserName", "حساب کاربری شما منقضی شده است");
                    _logger.LogInformation("Login failed: " + user.UserName + " Expired.");
                    return View(loginDTO);
                }
                else
                {
                    #region UseJWT
                    //var token = new UserTokens
                    //{
                    //    Id = Guid.NewGuid(),
                    //    UserId = user.Id,
                    //    IsDelete = false,
                    //    CreateHostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                    //};

                    //var tokenResult = await _tokenRepository.AddTokenAsync(token, cancellationToken);
                    //var jwt = await _jwtService.GenerateAsync(user, tokenResult);
                    #endregion

                    #region UseSignInAsync
                    var token = new UserToken
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        SessionId = HttpContext.Session.Id,
                        UserAgent = Request.Headers["User-Agent"].ToString(),
                        IsDelete = false,
                        CreateDate = DateTime.Now,
                        ExpirationDate = DateTime.Now.AddMinutes(30),
                        CreateHostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                    };

                    await _userTokenRepository.AddTokenAsync(token, cancellationToken);
                    user.UserTokens = await _userTokenRepository.GetByUserIdAsync(user.Id, cancellationToken);
                    var CurentView_UserMenubar = _view_UserMenubarRepository.GetUserAccessMenu(user.Id, cancellationToken);
                    string CurentView_UserMenubar_Json = JsonConvert.SerializeObject(CurentView_UserMenubar);

                    //List<View_UserMenubar> result = await _view_UserMenubarRepository.GetUserMenuByUser_Id(user.Id, cancellationToken);
                    //var claims = new List<Claim>()
                    //{
                    //    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    //    new Claim(ClaimTypes.Name,user.UserProfiles.Select(p=>p.FirstName).First()+" "+user.UserProfiles.Select(p=>p.LastName).First()),
                    //    new Claim("RoleID",user.UserRoles.Select(p=>p.Role.NormalizedRoleTitleEn).Last()),
                    //    new Claim("RoleTitle_Fa",user.UserRoles.Select(p=>p.Role.RoleTitleFa).First()),
                    //    new Claim("View_UserMenubar",son),
                    //};

                    var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserProfiles.Any() ? user.UserProfiles.First().FirstName + " " + user.UserProfiles.First().LastName : "No Profile"),
                            new Claim("RoleID", user.UserRoles.Any() ? user.UserRoles.Last().Role.NormalizedRoleTitleEn : "No Role"),
                            new Claim("RoleTitle_Fa", user.UserRoles.Any() ? user.UserRoles.First().Role.RoleTitleFa : "No Role"),
                            new Claim("View_UserMenubar", CurentView_UserMenubar_Json),
                        };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var pricipal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties()
                    {
                        IsPersistent = loginDTO.RemmeberMe
                    };

                    await HttpContext.SignInAsync(pricipal, authProperties);
                    #endregion

                    _logger.LogInformation("Login succeeded: " + user.UserName + " authenticated successfully.");
                    return Redirect("/Admin/Home");
                }
            }
            return View(loginDTO);
        }

        //[Authorize(Policy = "Over18")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDto, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var userId = new Guid();

                #region Add data in Users with RegisterDto from UI
                var user = registerDto.ToEntity(_mapper);
                user.Id = userId;
                user.UserName = registerDto.NationalCode.FixTextUpper();
                user.NormalizedUserName = registerDto.NationalCode.ToUpper().Trim();
                user.PasswordHash = PasswordHelper.HashPasswordBCrypt(registerDto.NationalCode);
                user.TwoFactorEnabled = false;
                user.PhoneNumberConfirmed = false;
                user.Creator = userId;
                user.HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                await _userRepository.AddAsync(user, cancellationToken);
                #endregion

                #region Add data in UserRoles with UserId from Users table
                UserRole userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = 1,
                    IsActive = true,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    Creator = user.Id,
                    HostIp = "::1"
                };
                _eFUserRoleRepository.Add(userRole);
                #endregion

                #region Add data in UserProfile with UserId from Users table
                var userProfiles = new UserProfile
                {
                    UserId = user.Id,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    NationalCode = registerDto.NationalCode,
                    IsActive = true,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    Creator = user.Id,
                    HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                };
                await _userProfilesRepository.AddAsync(userProfiles, cancellationToken);

                #endregion
            }
            return View(registerDto);
        }

        [AllowAnonymous]
        public IActionResult RecoveryPassword()
        {
            var dd = User.Identity.Name;
            //var dd1 = User.FindFirst("jwt").Value;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoveryPassword(RecoveryPasswordDTO recoveryPasswordDTO, CancellationToken cancellationToken)
        {
            var dd = recoveryPasswordDTO;
            return View(dd);
        }

        public IActionResult NoPermission()
        {
            return View();
        }

        public bool CheckNationalCode(string nationalCode)
        {
            if (nationalCode == null)
            {
                return false;
            }
            return false;// _context.IsExistNationalCode(nationalCode.ToLower().Trim()).Result;
        }

        public bool CheckMobile(string mobile)
        {
            if (mobile == null)
            {
                return false;
            }
            return false;//_context.IsExistMobile(mobile.ToLower().Trim()).Result;
        }



        [HttpGet("[action]")]
        public async Task<UserAccessMenuWithMenubar> MenuBar_WithUserSelectedAccess(User user, UserToken userToken, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new BadRequestException("کاربری انتخاب نشده است.");
            }

            var tokenId = userToken.UserId;
            List<View_UserMenubar> result1 = await _view_UserMenubarRepository.GetUserSelectedAccessMenuByUserCurrentIdAndUSerSelectedID(user.Id, userToken.UserId, cancellationToken);
            List<MenuSelectTreeJsDto> FinalList = new List<MenuSelectTreeJsDto>();
            foreach (var item in result1)
            {
                if (item.ParentId == null)
                {
                    if (item.CurentUser_Group_Checkecd == true || item.CurentUser_Person_Checkecd == true)
                    {
                        bool dis = false;
                        if (item.Group_Checkecd == true && item.IsParent == 0)
                        {
                            dis = true;
                        }
                        FinalList.Add(new MenuSelectTreeJsDto
                        {

                            key = item.Id.ToString(),
                            title = item.Title,
                            //disabled = dis,
                            disableCheckbox = dis,
                            children = GetChildMenu(item.Id, result1),

                        });
                    }
                }
            }
            List<string> acceess = new List<string>();
            foreach (var item in result1)
            {
                if ((item.Person_Checkecd == true || item.Group_Checkecd == true) && item.IsParent == 0 && (item.PageCode != 0 && item.PageCode != 100))
                {
                    acceess.Add(item.Id.ToString());
                }
            }
            UserAccessMenuWithMenubar userAccessMenuWithMenubar = new UserAccessMenuWithMenubar
            {
                menuSelectTreeJsDtos = FinalList,
                UserAccessMenu = acceess,
            };
            return userAccessMenuWithMenubar;
        }

        private List<MenuSelectTreeJsDto> GetChildMenu(Guid _id, List<View_UserMenubar> _List)
        {
            List<MenuSelectTreeJsDto> FinalList = new List<MenuSelectTreeJsDto>();
            foreach (var item in _List)
            {
                if (item.ParentId == _id)
                {
                    if (item.CurentUser_Group_Checkecd == true || item.CurentUser_Person_Checkecd == true)
                    {
                        bool dis = false;
                        if (item.Group_Checkecd == true && item.IsParent == 0)
                        {
                            dis = true;
                        }
                        FinalList.Add(new MenuSelectTreeJsDto
                        {

                            key = item.Id.ToString(),
                            title = item.Title,
                            //disabled = dis,
                            disableCheckbox = dis,
                            children = GetChildMenu(item.Id, _List)
                        });
                    }
                }
            }
            return FinalList;
        }
    }
}