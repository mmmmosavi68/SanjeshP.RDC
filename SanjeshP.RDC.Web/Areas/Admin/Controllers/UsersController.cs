using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Data;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Web.Areas.Admin.Models.UserViewModel;

namespace SanjeshP.RDC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UsersController(ApplicationDbContext context, IMapper mapper, IUserRepository userRepository)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var users = await _userRepository.TableNoTracking.ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                                                             .ToListAsync(cancellationToken);
            return View(users);
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,EmailAddress,Password,PhoneNumber")] UserAddViewModel userAddViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User() {
                    Id = Guid.NewGuid(),
                    UserName = userAddViewModel.UserName,
                    NormalizedUserName = userAddViewModel.UserName.ToUpper(),
                    EmailAddress = userAddViewModel.EmailAddress,
                    NormalizedEmailAddress = userAddViewModel.EmailAddress.ToUpper(),
                    EmailAddressConfirmed = false,
                    PasswordHash = SecurityHelper.GetSha256Hash(userAddViewModel.Password),
                    SecurityStamp = Guid.NewGuid(),
                    PhoneNumber = userAddViewModel.PhoneNumber,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    IsActive = true,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    ExpireDate = DateTime.Now.AddYears(1),
                    Creator = new Guid("00000000-0000-0000-0000-000000000001"),
                    HostIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userAddViewModel);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserName,NormalizedUserName,EmailAddress,NormalizedEmailAddress,EmailAddressConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,IsActive,IsDelete,LastLoginDate,CreateDate,ExpireDate,Creator,HostIp,Id")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
