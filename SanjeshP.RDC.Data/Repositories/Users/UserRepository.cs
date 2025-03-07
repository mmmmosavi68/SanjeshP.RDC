using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Convertor;
using SanjeshP.RDC.Data.Contracts.Users;
using SanjeshP.RDC.Data.Repositories.Common;
using SanjeshP.RDC.DTO.Users;
using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories.Users
{
    public class UserRepository : EFRepository<User>, IUserRepository, IScopedDependency
    {
        public UserRepository(ApplicationDbContext dbContext)
             : base(dbContext)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersNoTrackingAsync(CancellationToken cancellationToken)
        {
            return await TableNoTracking.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserByUserNameAsync(string userNameUpper, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.NormalizedUserName == userNameUpper.FixTextUpper())
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public User GetUserByUserName(string userNameUpper)
        {
            return Table.Where(p => p.NormalizedUserName == userNameUpper.FixTextUpper())
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .SingleOrDefault();
        }

        public async Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserByEmailAsync(string emailUpper, CancellationToken cancellationToken)
        {
            return await Table.AsNoTracking()
                .Where(u => u.IsDeleted == false && u.NormalizedEmailAddress == emailUpper)
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public User GetUserByEmail(string emailUpper)
        {
            return TableNoTracking.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .Where(p => p.NormalizedEmailAddress == emailUpper)
                .SingleOrDefault();
        }

        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .Where(p => p.PhoneNumber == phoneNumber)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
            return Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .Where(p => p.PhoneNumber == phoneNumber)
                .SingleOrDefault();
        }

        public async Task UpdateUserSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid();
            await UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public override void Update(User entity, bool saveNow = true)
        {
            entity.SecurityStamp = Guid.NewGuid();
            base.Update(entity, saveNow);
        }

        public async Task UpdateUserLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            await UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateUserAsync(User user, Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
        {
            user.IsDeleted = true;
            await UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<User> GetUserByUserNameAndPasswordAsync(string userName, string passwordHash, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.UserName == userName && p.PasswordHash == passwordHash)
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDeleted == false)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetByGuidIdAsync(Guid id,CancellationToken cancellationToken)
        {


            return await TableNoTracking.Include(ur => ur.UserRoles)
               .ThenInclude(r => r.Role)
               .Include(up => up.UserProfiles)
               .Where(u => u.IsDeleted == false && u.Id==id)
               .FirstOrDefaultAsync(cancellationToken)
               .ConfigureAwait(false);

            //var user = await Table
            //        .Include(u => u.UserProfiles)
            //        .FirstOrDefaultAsync(u => u.Id == id);
            //return user;
        }


        // افزودن کاربر
        public async Task<Guid> AddUserAsync(AddUserDto userDto)
        {
            //var user = new User
            //{
            //    Id = Guid.NewGuid(),
            //    UserName = userDto.Username,
            //    EmailAddress = userDto.Email,
            //    IsActive = userDto.IsActive,
            //    CreatedDate = DateTime.Now
            //};

            //var userProfile = new UserProfile
            //{
            //    UserId = user.Id,
            //    FirstName = userDto.FirstName,
            //    LastName = userDto.LastName,
            //    NationalCode = userDto.NationalCode
            //};

            //var userRole = new UserRole
            //{
            //    UserId = user.Id,
            //    RoleId = userDto.RoleId
            //};

            //await _context.Users.AddAsync(user);
            //await _context.UserProfiles.AddAsync(userProfile);
            //await _context.UserRoles.AddAsync(userRole);
            //await _context.SaveChangesAsync();

            //return user.Id;
            return Guid.NewGuid();
        }

        // ویرایش کاربر
        public async Task<bool> UpdateUserAsync(EditUserDto userDto)
        {
            //var user = await TableNoTracking.FindAsync(userDto.UserId);
            //if (user == null) return false;

            //user.Username = userDto.Username;
            //user.Email = userDto.Email;
            //user.IsActive = userDto.IsActive;

            //var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userDto.UserId);
            //if (userProfile != null)
            //{
            //    userProfile.FirstName = userDto.FirstName;
            //    userProfile.LastName = userDto.LastName;
            //    userProfile.NationalCode = userDto.NationalCode;
            //}

            //var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == userDto.UserId);
            //if (userRole != null)
            //{
            //    userRole.RoleId = userDto.RoleId;
            //}

            //await _context.SaveChangesAsync();
            return true;
        }

        // حذف کاربر
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            //var user = await _context.Users.FindAsync(userId);
            //if (user == null) return false;

            //_context.Users.Remove(user);

            //var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            //if (userProfile != null)
            //{
            //    _context.UserProfiles.Remove(userProfile);
            //}

            //var userRoles = await _context.UserRoles.Where(r => r.UserId == userId).ToListAsync();
            //if (userRoles.Any())
            //{
            //    _context.UserRoles.RemoveRange(userRoles);
            //}

            //await _context.SaveChangesAsync();
            return true;
        }

        // دریافت اطلاعات کاربر
        public async Task<UserDetailsDto> GetUserDetailsAsync(Guid userId)
        {
            //var user = await (from u in _context.Users
            //                  join p in _context.UserProfiles on u.Id equals p.UserId
            //                  join ur in _context.UserRoles on u.Id equals ur.UserId
            //                  join r in _context.Roles on ur.RoleId equals r.Id
            //                  where u.Id == userId
            //                  select new UserDetailsDto
            //                  {
            //                      UserId = u.Id,
            //                      Username = u.Username,
            //                      Email = u.Email,
            //                      FullName = $"{p.FirstName} {p.LastName}",
            //                      RoleName = r.Name,
            //                      IsActive = u.IsActive,
            //                      CreatedDate = u.CreatedDate,
            //                      LastLoginDate = u.LastLoginDate
            //                  }).FirstOrDefaultAsync();

            return null;
        }

        // دریافت لیست کاربران
        public async Task<List<UserListDto>> GetUsersAsync()
        {
            //var users = await (from u in _context.Users
            //                   join p in _context.UserProfiles on u.Id equals p.UserId
            //                   join ur in _context.UserRoles on u.Id equals ur.UserId
            //                   join r in _context.Roles on ur.RoleId equals r.Id
            //                   select new UserListDto
            //                   {
            //                       UserId = u.Id,
            //                       Username = u.Username,
            //                       Email = u.Email,
            //                       FullName = $"{p.FirstName} {p.LastName}",
            //                       RoleName = r.Name,
            //                       IsActive = u.IsActive,
            //                       LastLoginDate = u.LastLoginDate
            //                   }).ToListAsync();

            return null;
        }
    }
}
