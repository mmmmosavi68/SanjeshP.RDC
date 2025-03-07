using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts.Users;
using SanjeshP.RDC.Data.Repositories.Common;
using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories.Users
{
    public class UserRoleRepository : EFRepository<UserRole>, IUserRoleRepository, IScopedDependency
    {
        public UserRoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public UserRole GetUserRoleByUserId(Guid userId)
        {
            return TableNoTracking.Where(ur => ur.IsDeleted == false && ur.UserId == userId).FirstOrDefault();
        }
    }
}
