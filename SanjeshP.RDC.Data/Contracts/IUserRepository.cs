using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Common;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IUserRepository : IEFRepository<User>,IScopedDependency
    {
        public Task<IEnumerable<User>> GetByAllNoTrackingAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<User>> GetByAllAsync(CancellationToken cancellationToken);
        public Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
        public Task<User> GetByUserNameAndPasswordAsync(string userName,string passwordHash, CancellationToken cancellationToken);
        public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
        public Task<User> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
        public Task UpdateAsync(User user, Guid id, CancellationToken cancellationToken);
        public void DeleteAsync(User user, CancellationToken cancellationToken);
        public Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken);
        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}
