using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User;
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
    }
}
