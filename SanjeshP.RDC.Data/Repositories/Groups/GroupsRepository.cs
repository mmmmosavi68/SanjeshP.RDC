using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts.Groups;
using SanjeshP.RDC.Data.Repositories.Common;
using SanjeshP.RDC.Entities.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanjeshP.RDC.Data.Repositories.Groups
{
    public class GroupRepository : EFRepository<Group>, IGroupRepository, IScopedDependency
    {
        public GroupRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public IReadOnlyList<Group> GetAllGroups()
        {
            return TableNoTracking
                    .Where(g => g.IsDeleted != true)
                    .Include(g => g.Creator)
                    .ToList();
        }
    }
}
