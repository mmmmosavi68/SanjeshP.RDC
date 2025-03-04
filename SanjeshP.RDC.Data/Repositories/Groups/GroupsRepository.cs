using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts.Groups;
using SanjeshP.RDC.Entities.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanjeshP.RDC.Data.Repositories.Groups
{
    public class GroupsRepository : EFRepository<Group>, IGroupsRepository, IScopedDependency
    {
        public GroupsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public IReadOnlyList<Group> GetAll()
        {
            return TableNoTracking
                    .Where(g => g.IsDelete != true)
                    .Include(g => g.Creator)
                    .ToList();
        }
    }
}
