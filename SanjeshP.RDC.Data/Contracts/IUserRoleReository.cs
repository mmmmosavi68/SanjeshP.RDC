using SanjeshP.RDC.Common;
using SanjeshP.RDC.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IUserRoleReository : IEFRepository<UserRole>
    {
        public UserRole GetByUserId(Guid userId);
    }
}
