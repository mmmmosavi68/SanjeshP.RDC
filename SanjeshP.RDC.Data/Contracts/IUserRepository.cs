using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Common;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IUserRepository : IEFRepository<User>
    {
        public Task<IEnumerable<User>> GetByAllNoTrackingAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<User>> GetByAllAsync(CancellationToken cancellationToken);
        public Task<User> GetByUserNameAsync(string userNameUpper, CancellationToken cancellationToken);
        public User GetByUserName(string userNameUpper);
        public Task<User> GetByUserNameAndPasswordAsync(string userName,string passwordHash, CancellationToken cancellationToken);
        public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<User> GetByEmailAsync(string emailUpper, CancellationToken cancellationToken);
        public User GetByEmail(string emailUpper );
        public Task<User> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
        public User GetByPhoneNumber(string phoneNumber );
        public Task UpdateAsync(User user, Guid id, CancellationToken cancellationToken);
        public Task DeleteAsync(User user, CancellationToken cancellationToken);
        public Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken);
        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}
