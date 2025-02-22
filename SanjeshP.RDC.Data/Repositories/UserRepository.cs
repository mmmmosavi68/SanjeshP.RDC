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
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<User>> GetByAllAsync(CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .ToListAsync(cancellationToken);
        }
        public async Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.NormalizedUserName == userName.FixTextUpper())
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .Where(p => p.Id == id).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .Where(p => p.EmailAddress == email).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<User> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .Where(p => p.PhoneNumber == phoneNumber).SingleOrDefaultAsync(cancellationToken);
        }

        public Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid();
            return UpdateAsync(user, cancellationToken);
        }

        public override void Update(User entity, bool saveNow = true)
        {
            entity.SecurityStamp = Guid.NewGuid();
            base.Update(entity, saveNow);
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }


        public Task UpdateAsync(User user, Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(User User, CancellationToken cancellationToken)
        {
            User.IsDelete = true;
            Update(User, true);
        }

        public async Task<User> GetByUserNameAndPasswordAsync(string userName, string passwordHash, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.UserName == userName && p.PasswordHash == passwordHash)
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
