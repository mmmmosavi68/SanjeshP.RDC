using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SanjeshP.RDC.Data.Repositories
{
    public class AccessMenuRepository : EFRepository<AccessMenus>, IAccessMenuRepository, IScopedDependency
    {
        protected readonly ApplicationDbContext _dbContext;
        public AccessMenuRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<AccessMenus>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            // Fore modify UserAccessMenus table
            // IsDelete dont checked
            return await TableNoTracking.Where(u => u.UserId.Equals(userId)).ToListAsync(cancellationToken);
        }

    }
}
