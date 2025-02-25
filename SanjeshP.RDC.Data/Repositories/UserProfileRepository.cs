using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories
{
    public class UserProfileRepository : EFRepository<UserProfile>, IUserProfilesRepository, IScopedDependency
    {
        public UserProfileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<UserProfile> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.Id == id).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<UserProfile> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.UserId == userId).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<UserProfile> GetByCodeMeliAsync(string nationalCode, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.NationalCode == nationalCode).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task AddByUserIdAsync(UserProfile userProfile, Guid userId, CancellationToken cancellationToken)
        {
            userProfile.UserId = userId;
            await base.AddAsync(userProfile, cancellationToken);
        }
        public async Task UpdateAsync(UserProfile user, int Id, CancellationToken cancellationToken)
        {
            await Table.Where(p => p.Id == Id).SingleOrDefaultAsync(cancellationToken);

        }
        public async Task DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
