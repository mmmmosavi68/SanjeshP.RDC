using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IUserTokenRepository : IEFRepository<UserToken>
    {
        public Task<List<UserToken>> GetByUserIdAsync(Guid userID, CancellationToken cancellationToken);
        public Task<UserToken> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task AddTokenAsync(UserToken tokens, CancellationToken cancellationToken, bool saveNow = true);
        public Task UpdateDateTimeAsync(Guid id, CancellationToken cancellationToken);
        public Task DeleteByIDAsync(Guid id, CancellationToken cancellationToken);
        public Task DeleteByUserIDAsync(Guid id, CancellationToken cancellationToken);

    }
}
