using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Convertor;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository, IScopedDependency
    {
        public UserRepository(ApplicationDbContext dbContext)
             : base(dbContext)
        {
        }

        public async Task<IEnumerable<User>> GetByAllNoTrackingAsync(CancellationToken cancellationToken)
        {
            return await TableNoTracking.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetByAllAsync(CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetByUserNameAsync(string userNameUpper, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.NormalizedUserName == userNameUpper.FixTextUpper())
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public User GetByUserName(string userNameUpper)
        {
            return Table.Where(p => p.NormalizedUserName == userNameUpper.FixTextUpper())
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .SingleOrDefault();
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetByEmailAsync(string emailUpper, CancellationToken cancellationToken)
        {
            return await Table.AsNoTracking()
                .Where(u => u.IsDelete == false && u.NormalizedEmailAddress == emailUpper)
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public User GetByEmail(string emailUpper)
        {
            return TableNoTracking.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .Where(p => p.NormalizedEmailAddress == emailUpper)
                .SingleOrDefault();
        }

        public async Task<User> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .Where(p => p.PhoneNumber == phoneNumber)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public User GetByPhoneNumber(string phoneNumber)
        {
            return Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .Where(p => p.PhoneNumber == phoneNumber)
                .SingleOrDefault();
        }

        public async Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid();
            await UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public override void Update(User entity, bool saveNow = true)
        {
            entity.SecurityStamp = Guid.NewGuid();
            base.Update(entity, saveNow);
        }

        public async Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            await UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(User user, Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(User user, CancellationToken cancellationToken)
        {
            user.IsDelete = true;
            await UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<User> GetByUserNameAndPasswordAsync(string userName, string passwordHash, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.UserName == userName && p.PasswordHash == passwordHash)
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}