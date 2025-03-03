using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IAccessMenuRepository : IEFRepository<AccessMenus>
    {
        public Task<List<AccessMenus>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
