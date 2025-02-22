using SanjeshP.RDC.Common;
using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SanjeshP.RDC.Data.Repositories
{
    public class TokenRepository : EFRepository<UserToken>, IUserTokenRepository, IScopedDependency
    {
        public TokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<UserToken> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.Id == id)
                .SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<List<UserToken>> GetByUserIdAsync(Guid userID, CancellationToken cancellationToken)
        {
            return await Table.Where(p => p.UserId == userID && p.IsDelete == false).ToListAsync();
        }
        public async Task AddTokenAsync(UserToken tokens, CancellationToken cancellationToken, bool saveNow = true)
        {
            await DeleteByUserIDAsync(tokens.UserId, cancellationToken);
            await base.AddAsync(tokens, cancellationToken);
            //var dd = await TableNoTracking.Where(ut => ut.IsDelete == false).SingleOrDefaultAsync();
            //return await TableNoTracking.Where(ut => ut.IsDelete == false).SingleOrDefaultAsync();
        }
        public async Task UpdateDateTimeAsync(Guid id, CancellationToken cancellationToken)
        {
            var tokens = await GetByIdAsync(id, cancellationToken);
            tokens.CreateDate = DateTime.Now;
            await UpdateAsync(tokens, cancellationToken);
        }
        public async Task DeleteByIDAsync(Guid id, CancellationToken cancellationToken)
        {
            var tokens = await GetByIdAsync(id, cancellationToken);
            tokens.IsDelete = true;
            await UpdateAsync(tokens, cancellationToken);
        }
        public async Task DeleteByUserIDAsync(Guid userId, CancellationToken cancellationToken)
        {
            var tokens = await GetByUserIdAsync(userId, cancellationToken);
            if (tokens != null)
            {
                foreach (var item in tokens)
                {
                    item.IsDelete = true;
                    await UpdateAsync(item, cancellationToken);
                }

            }
        }
    }
}
