using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Data.Contracts.Common;

namespace SanjeshP.RDC.Data.Contracts.Menus
{
    public interface IAccessMenusRepository : IEntityFrameworkRepository<UserAccessMenus>
    {
        public Task<List<UserAccessMenus>> GetUserAccessMenusByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
