using SanjeshP.RDC.Common;
using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Data.Contracts.Users;
using SanjeshP.RDC.Data.Repositories.Common;

namespace SanjeshP.RDC.Data.Repositories.Users
{
    public class UserTokenRepository : EFRepository<UserToken>, IUserTokenRepository, IScopedDependency
    {
        public UserTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserToken> GetUserTokenByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.Id == id)
                .Include(p => p.User)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<List<UserToken>> GetUserTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.UserId == userId && p.IsDeleted == false).Include(p => p.User).ToListAsync(cancellationToken);
        }

        public async Task AddUserTokenAsync(UserToken token, CancellationToken cancellationToken, bool saveNow = true)
        {
            await DeleteUserTokensByUserIdAsync(token.UserId, cancellationToken);
            await base.AddAsync(token, cancellationToken);
        }

        public async Task UpdateTokenDateTimeAsync(Guid id, CancellationToken cancellationToken)
        {
            var token = await GetUserTokenByIdAsync(id, cancellationToken);
            token.CreatedDate = DateTime.Now;
            await UpdateAsync(token, cancellationToken);
        }

        public async Task DeleteUserTokenByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var token = await GetUserTokenByIdAsync(id, cancellationToken);
            token.IsDeleted = true;
            await UpdateAsync(token, cancellationToken);
        }

        public async Task DeleteUserTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var tokens = await GetUserTokensByUserIdAsync(userId, cancellationToken);
            if (tokens != null)
            {
                foreach (var token in tokens)
                {
                    token.IsDeleted = true;
                    await UpdateAsync(token, cancellationToken);
                }
            }
        }

        public Task GetTokenByCodeAsync(string verificationCode, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
