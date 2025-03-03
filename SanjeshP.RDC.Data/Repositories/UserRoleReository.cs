using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories
{
    public class UserRoleReository : EFRepository<UserRole>, IUserRoleReository, IScopedDependency
    {
        public UserRoleReository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public UserRole GetByUserId(Guid userId)
        {
            return TableNoTracking.Where(ur => ur.IsDelete == false && ur.UserId == userId).FirstOrDefault();
        }
    }
}
