using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Common.Utilities;
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

        public async Task<IEnumerable<User>> GetAll()
        {
            return await TableNoTracking.Where(u => u.IsDelete == false).ToListAsync();
        }
        public Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return Table.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Table.Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .Include(up => up.UserProfiles)
                .Where(u => u.IsDelete == false)
                .Where(p => p.Id == id).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            // بهتره تو لایه سرویس بررسی شود
            var exist = await TableNoTracking.AnyAsync(p => p.UserName == user.UserName);
            if (exist)
                throw new BadRequestException("نام کاربری تکراری است");

            var passwordHash = SecurityHelper.GetSha256Hash(password) ?? string.Empty;
            user.PasswordHash = passwordHash;
            await base.AddAsync(user, cancellationToken);
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }

        public Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid();
            return UpdateAsync(user, cancellationToken);
        }


        //public override void Update(User entity, bool saveNow = true)
        //{
        //    entity.SecurityStamp = Guid.NewGuid();
        //    base.Update(entity, saveNow);
        //}
    }
}
