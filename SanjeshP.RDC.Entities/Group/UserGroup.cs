using SanjeshP.RDC.Entities.Common;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Entities.Group
{
    public class UserGroup : BaseEntity<Guid>
    {
        public UserGroup()
        {
            IsActive = true;
            IsDelete = false;
            CreateDate = DateTime.Now;
        }
        //public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GroupId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual ICollection<AccessMenusGroup> AccessMenusGroups { get; set; } = new List<AccessMenusGroup>();
        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}
