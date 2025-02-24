using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using SanjeshP.RDC.Convertor;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User;
using SanjeshP.RDC.WebFramework.UserAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace SanjeshP.RDC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfilesRepository _userProfilesRepository;
        private readonly IEFRepository<Role> _eFRepositoryRole;
        private readonly IEFRepository<Menu> _eFRepositoryListMenu;
        private readonly IView_UserMenubarRepository _view_UserMenubarRepository;

        public UsersController(IMapper mapper
                                , ILogger<UsersController> logger
                                , IUserRepository userRepository
                                , IUserProfilesRepository userProfilesRepository
                                , IEFRepository<Role> eFRepositoryRole
                                , IEFRepository<Menu> eFRepositoryListMenu
                                , IView_UserMenubarRepository view_UserMenubarRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _userProfilesRepository = userProfilesRepository;
            _eFRepositoryRole = eFRepositoryRole;
            _eFRepositoryListMenu = eFRepositoryListMenu;
            _view_UserMenubarRepository = view_UserMenubarRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByAllNoTrackingAsync(cancellationToken);

            List<RegisterDto> newList = users.Select(user => new RegisterDto
            {
                UserId = user.Id,
                FirstName = user.UserProfiles.Select(p => p.FirstName).First(),
                LastName = user.UserProfiles.Select(p => p.LastName).First(),
                NationalCode = user.UserProfiles.Select(p => p.NationalCode).First(),
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
                FirstName = user.UserProfiles.Select(p => p.FirstName).First(),
                LastName = user.UserProfiles.Select(p => p.LastName).First(),
                NationalCode = user.UserProfiles.Select(p => p.NationalCode).First(),
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


        public IActionResult AddUser(CancellationToken cancellationToken)
        {
            return PartialView("AddUser");
        }
        public async Task<IActionResult> AddUser(RegisterEditDto registerEditDto, CancellationToken cancellationToken)
        {
            return PartialView("AddUser");
        }
        [HttpGet]
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
            var registerEditDto = new RegisterEditDto
            {
                
                UserId = user.Id,
                FirstName = user.UserProfiles.Select(p => p.FirstName).First(),
                LastName = user.UserProfiles.Select(p => p.LastName).First(),
                NationalCode = user.UserProfiles.Select(p => p.NationalCode).First(),
                UserName = user.UserName,
                Password = string.Empty,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.UserRoles.Select(p => p.RoleId).Last(),
                IsActive = user.IsActive,
            };

            return PartialView("EditUser", registerEditDto);
        }
        [HttpPost]
        public IActionResult EditUser(RegisterEditDto registerEditDto, Guid userid, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetById(userid);
                var userProfile = _userProfilesRepository.GetById(userid);

                if(user.NormalizedUserName != registerEditDto.UserName.FixTextUpper())
                {
                    var userExist = _userRepository.GetByUserNameAsync(user.NormalizedUserName,cancellationToken);
                    if(userExist != null)
                    {
                        ModelState.AddModelError("UserName", "ایمیل تکراری است");
                        return PartialView("EditUser", registerEditDto);
                    }
                }
                else if (user.NormalizedEmailAddress != registerEditDto.EmailAddress.FixTextUpper())
                {

                }
                else if(user.PhoneNumber != registerEditDto.PhoneNumber.FixTextUpper())
                {
                    
                }
                else
                {

                }
            }
            return PartialView("EditUser");
        }
    }
}
