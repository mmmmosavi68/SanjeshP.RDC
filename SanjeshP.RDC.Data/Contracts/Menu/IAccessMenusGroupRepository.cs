using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SanjeshP.RDC.Data.Contracts.Menu
{
    public interface IAccessMenusGroupRepository : IEFRepository<AccessMenusGroup>
    {
        public Task<List<AccessMenusGroup>> GetAllByGroupIdAsync(Guid groupID, CancellationToken cancellationToken);
    }
}
