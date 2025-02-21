using SanjeshP.RDC.Entities.Common;
using SanjeshP.RDC.Entities.Group;
using System;

namespace SanjeshP.RDC.Entities.Menu
{
    public class AccessMenusGroup : BaseEntity
    {
        public AccessMenusGroup()
        {
            IsActive = true;
            IsDelete = false;
            CreateDate = DateTime.Now;
        }
        //public int Id { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? ListMenuId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual Menu ListMenu { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
