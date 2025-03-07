using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Data.Contracts.Common;

namespace SanjeshP.RDC.Data.Contracts.Menus
{
    public interface IMenusRepository : IEntityFrameworkRepository<Entities.Menu.Menu>
    {
        public Task<List<Entities.Menu.Menu>> GetAllMenusAsync(CancellationToken cancellationToken);
    }
}
