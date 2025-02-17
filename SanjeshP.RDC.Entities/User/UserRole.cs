using SanjeshP.RDC.Entities.Common;
using System;

namespace SanjeshP.RDC.Entities.User
{
    public class UserRole : BaseEntity
    {
       //public int Id { get; set; }
        public Guid? UserId { get; set; }
        public int? RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
