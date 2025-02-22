using SanjeshP.RDC.Entities.Common;
using System;
using SanjeshP.RDC.Entities.User;

namespace SanjeshP.RDC.Entities.Menu
{
    public class AccessMenus : BaseEntity
    {
        public AccessMenus()
        {
            IsActive = true;
            IsDelete = false;
            CreateDate = DateTime.Now;
        }
        //public int Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ListMenuId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual Menu ListMenu { get; set; }
        public virtual User.User User { get; set; }
    }
}
