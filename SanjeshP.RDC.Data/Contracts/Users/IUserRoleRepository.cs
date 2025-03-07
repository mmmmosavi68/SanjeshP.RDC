using SanjeshP.RDC.Data.Contracts.Common;
using SanjeshP.RDC.Entities.User;
using System;

namespace SanjeshP.RDC.Data.Contracts.Users
{
    public interface IUserRoleRepository : IEntityFrameworkRepository<UserRole>
    {
        public UserRole GetUserRoleByUserId(Guid userId);
    }
}
