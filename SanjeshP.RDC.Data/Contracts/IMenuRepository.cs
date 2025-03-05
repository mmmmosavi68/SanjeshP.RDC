using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IMenuRepository : IEFRepository<SanjeshP.RDC.Entities.Menu.Menu>
    {
        public Task<List<SanjeshP.RDC.Entities.Menu.Menu>> GetAllMenu(CancellationToken cancellationToken);
    }
}
