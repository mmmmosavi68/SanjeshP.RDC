using Microsoft.EntityFrameworkCore;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts.Menus;
using SanjeshP.RDC.Data.Repositories.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Repositories.Menus
{
    public class MenusRepository : EFRepository<Entities.Menu.Menu>, IMenusRepository, IScopedDependency
    {
        public MenusRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<Entities.Menu.Menu>> GetAllMenusAsync(CancellationToken cancellationToken)
        {
            return await TableNoTracking
                          .Where(ll => ll.IsDeleted == false)
                          .Include(rr => rr.MenusAccesses)
                          .OrderBy(ll => ll.PageCode) // مرتب‌سازی براساس CreatedDate به‌صورت نزولی
                          .ToListAsync(cancellationToken);
        }
    }
}
