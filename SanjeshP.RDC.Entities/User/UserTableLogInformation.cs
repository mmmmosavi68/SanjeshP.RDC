using SanjeshP.RDC.Entities.Common;

namespace SanjeshP.RDC.Entities.User
{
    public class UserTableLogInformation : BaseEntity
    {
        //public int Id { get; set; }

        public long? UserLgoTableId { get; set; }

        public string DataValue { get; set; }

        public int? UserLgoTableId1 { get; set; }

        public int? UserLgoTableId1NavigationId { get; set; }

        public virtual UserTablesLog UserLgoTableId1Navigation { get; set; }
    }
}
