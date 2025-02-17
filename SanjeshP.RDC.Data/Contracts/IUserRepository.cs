using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);
        public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(User user, string password, CancellationToken cancellationToken);
        Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}
