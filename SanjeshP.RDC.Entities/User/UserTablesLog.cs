using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Entities.User
{
    public class UserTablesLog : BaseEntity
    {
        //public int Id { get; set; }

        public Guid? UserId { get; set; }

        public string TableName { get; set; }

        public string RecordId { get; set; }

        public int? Opareation { get; set; }

        public string CreateHostIp { get; set; }

        public DateTime? CreateDate { get; set; }

        public string SystemInformation { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<UserTableLogInformation> UserTableLogInformations { get; set; } = new List<UserTableLogInformation>();
    }
}
