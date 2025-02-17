using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Entities.User
{
    public class Role : BaseEntity
    {
        //public int Id { get; set; }
        public string RoleTitleEn { get; set; }
        public string NormalizedRoleTitleEn { get; set; }
        public string RoleTitleFa { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public string Name { get; set; }
    }
}
