using SanjeshP.RDC.Data.Contracts.Common;

namespace SanjeshP.RDC.Data.Contracts.Groups
{
    public interface IUserGroupRepository : IEntityFrameworkRepository<SanjeshP.RDC.Entities.Group.GroupUsers>
    {
    }
}
