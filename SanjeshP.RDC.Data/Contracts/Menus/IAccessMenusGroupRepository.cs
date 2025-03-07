using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Data.Contracts.Common;

namespace SanjeshP.RDC.Data.Contracts.Menu
{
    public interface IAccessMenusGroupRepository : IEntityFrameworkRepository<GroupAccessMenus>
    {
        public Task<List<GroupAccessMenus>> GetGroupAccessMenusByGroupIdAsync(Guid groupId, CancellationToken cancellationToken);
    }
}
