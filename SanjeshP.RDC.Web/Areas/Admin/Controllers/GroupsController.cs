using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Data.Contracts.Groups;
using SanjeshP.RDC.Entities.Group;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Group;
using System.Collections.Generic;
using System.Threading;

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
        public GroupsController(IMapper mapper
                                , ICompositeViewEngine viewEngine
                                , ILogger<UsersController> logger
                                , IGroupsRepository groupsRepository
                                , IUserRepository userRepository)
        {
            _mapper = mapper;
            _viewEngine = viewEngine;
            _logger = logger;
            _groupsRepository = groupsRepository;
            _userRepository = userRepository;
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
    }
}
