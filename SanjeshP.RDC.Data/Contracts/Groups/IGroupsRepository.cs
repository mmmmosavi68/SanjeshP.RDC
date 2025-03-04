using System.Collections.Generic;

namespace SanjeshP.RDC.Data.Contracts.Groups
{
    public interface IGroupsRepository : IEFRepository<SanjeshP.RDC.Entities.Group.Group>
    {
        IReadOnlyList<SanjeshP.RDC.Entities.Group.Group> GetAll();
    }

}
