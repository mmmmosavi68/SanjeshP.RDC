using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts.Groups;
using SanjeshP.RDC.Data.Repositories.Common;
using SanjeshP.RDC.Entities.Group;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.Data.Repositories.Groups
{
    public class UserGroupRepository : EFRepository<GroupUsers>, IUserGroupRepository, IScopedDependency
    {
        public UserGroupRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
