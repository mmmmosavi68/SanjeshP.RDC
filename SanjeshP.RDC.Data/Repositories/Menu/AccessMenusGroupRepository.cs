using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts.Menu;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories.Menu
{
    public class AccessMenusGroupRepository : EFRepository<AccessMenusGroup>, IAccessMenusGroupRepository, IScopedDependency
    {
        public AccessMenusGroupRepository(ApplicationDbContext dbContext)
          : base(dbContext)
        {
        }

        public async Task<List<AccessMenusGroup>> GetAllByGroupIdAsync(Guid groupID, CancellationToken cancellationToken)
        {
            return await TableNoTracking.Where(u => u.GroupId.Equals(groupID)).ToListAsync(cancellationToken);
        }
    }
}
