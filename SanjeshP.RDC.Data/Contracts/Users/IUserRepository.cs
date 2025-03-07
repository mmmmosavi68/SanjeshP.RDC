using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Data.Contracts.Common;

namespace SanjeshP.RDC.Data.Contracts.Users
{
    public interface IUserRepository : IEntityFrameworkRepository<User>
    {
        Task<IEnumerable<User>> GetAllUsersNoTrackingAsync(CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);

        Task<User> GetByGuidIdAsync(Guid id,CancellationToken cancellationToken);
        Task<User> GetUserByUserNameAsync(string userNameUpper, CancellationToken cancellationToken);
        User GetUserByUserName(string userNameUpper);
        Task<User> GetUserByUserNameAndPasswordAsync(string userName, string passwordHash, CancellationToken cancellationToken);
        Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User> GetUserByEmailAsync(string emailUpper, CancellationToken cancellationToken);
        User GetUserByEmail(string emailUpper);
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
        User GetUserByPhoneNumber(string phoneNumber);
        Task UpdateUserAsync(User user, Guid id, CancellationToken cancellationToken);
        Task DeleteUserAsync(User user, CancellationToken cancellationToken);
        Task UpdateUserSecurityStampAsync(User user, CancellationToken cancellationToken);
        Task UpdateUserLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}
