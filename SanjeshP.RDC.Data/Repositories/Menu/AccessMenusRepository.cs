using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Data.Contracts.Menus;
using SanjeshP.RDC.Data.Repositories.Common;

namespace SanjeshP.RDC.Data.Repositories.Menus
{
    public class AccessMenusRepository : EFRepository<UserAccessMenus>, IAccessMenusRepository, IScopedDependency
    {
        protected readonly ApplicationDbContext _dbContext;
        public AccessMenusRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<UserAccessMenus>> GetUserAccessMenusByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            // Fore modify UserAccessMenus table
            // IsDelete don't checked
            return await TableNoTracking.Where(u => u.UserId.Equals(userId)).ToListAsync(cancellationToken);
        }

    }
}
