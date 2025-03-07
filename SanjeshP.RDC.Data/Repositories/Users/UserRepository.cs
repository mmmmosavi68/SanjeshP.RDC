using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Convertor;
using SanjeshP.RDC.Data.Contracts.Users;
using SanjeshP.RDC.Data.Repositories.Common;
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
    }
}
