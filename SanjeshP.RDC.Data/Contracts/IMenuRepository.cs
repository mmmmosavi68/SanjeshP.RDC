using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Entities.Menu;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IMenuRepository
    {
        public Task<List<Menu>> GetAllMenu(CancellationToken cancellationToken);
    }
}
