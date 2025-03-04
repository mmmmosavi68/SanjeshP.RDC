using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Data.Contracts.Groups;
using SanjeshP.RDC.Entities.Group;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Group;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.Collections.Generic;
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

        public GroupsController(IMapper mapper
                                , ICompositeViewEngine viewEngine
                                , ILogger<UsersController> logger
                                , IGroupsRepository groupsRepository
                                , IUserRepository userRepository
                                , IUserTokenRepository userTokenRepository
                                ,IEFRepository<UserGroup> eFRepositoryUserGroup)
        {
            _mapper = mapper;
            _viewEngine = viewEngine;
            _logger = logger;
            _groupsRepository = groupsRepository;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _eFRepositoryUserGroup = eFRepositoryUserGroup;
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
            await _groupsRepository.UpdateAsync(group,cancellationToken);

            return Ok();
        }


    }
}
