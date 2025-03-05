using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories
{
    public class MenuRepository : EFRepository<SanjeshP.RDC.Entities.Menu.Menu>, IMenuRepository, IScopedDependency
    {
        public MenuRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<SanjeshP.RDC.Entities.Menu.Menu>> GetAllMenu(CancellationToken cancellationToken)
        {
            return await TableNoTracking.Where(ll => ll.IsDelete == false).Include(rr => rr.MenusAccesses).ToListAsync(cancellationToken);

        }
    }
}
