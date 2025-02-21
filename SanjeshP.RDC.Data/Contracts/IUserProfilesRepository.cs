using System;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Entities.User;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IUserProfilesRepository : IEFRepository<UserProfile>
    {
        public Task<UserProfile> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<UserProfile> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        public Task<UserProfile> GetByCodeMeliAsync(string nationalCode, CancellationToken cancellationToken);
        //public Task AddAsync(UserProfiles userProfile, CancellationToken cancellationToken);
        public Task AddByUserIdAsync(UserProfile userProfile, Guid userId, CancellationToken cancellationToken);
        public Task UpdateAsync(UserProfile userProfile, int Id, CancellationToken cancellationToken);
        public Task DeleteAsync(string Id, CancellationToken cancellationToken);

    }
}
