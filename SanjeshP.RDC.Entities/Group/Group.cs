using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Entities.Group
{
    public class Group : BaseEntity<Guid>   
    {
        public Group()
        {
            IsActive = true;
            IsDelete = false;
            CreateDate = DateTime.Now;
        }
        //public Guid Id { get; set; }
        public string UserGroupText { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual ICollection<UserGroup> GroupUsers { get; set; } = new List<UserGroup>();
    }
}
