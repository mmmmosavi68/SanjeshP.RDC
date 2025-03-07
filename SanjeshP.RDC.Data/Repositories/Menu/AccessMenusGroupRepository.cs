using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts.Menu;
using SanjeshP.RDC.Data.Contracts.Menus;
using SanjeshP.RDC.Data.Repositories.Common;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories.Menus
{
    public class AccessMenusGroupRepository : EFRepository<GroupAccessMenus>, IAccessMenusGroupRepository, IScopedDependency
    {
        public AccessMenusGroupRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<GroupAccessMenus>> GetGroupAccessMenusByGroupIdAsync(Guid groupId, CancellationToken cancellationToken)
        {
            return await TableNoTracking.Where(u => u.GroupId.Equals(groupId)).ToListAsync(cancellationToken);
        }
    }
}
