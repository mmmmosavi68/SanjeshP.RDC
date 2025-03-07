using System;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Data.Contracts.Common;

namespace SanjeshP.RDC.Data.Contracts.Users
{
    public interface IUserProfileRepository : IEntityFrameworkRepository<UserProfile>
    {
        Task<UserProfile> GetProfileByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserProfile> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserProfile> GetProfileByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken);
        Task AddUserProfileAsync(UserProfile userProfile, Guid userId, CancellationToken cancellationToken);
        Task UpdateUserProfileAsync(UserProfile userProfile, int id, CancellationToken cancellationToken);
        Task DeleteUserProfileAsync(Guid id, CancellationToken cancellationToken);
    }
}
