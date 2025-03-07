using SanjeshP.RDC.Data.Contracts.Common;
using SanjeshP.RDC.Entities.Group;
using System.Collections.Generic;

namespace SanjeshP.RDC.Data.Contracts.Groups
{
    public interface IGroupRepository : IEntityFrameworkRepository<Group>
    {
        IReadOnlyList<Group> GetAllGroups();
    }

}
