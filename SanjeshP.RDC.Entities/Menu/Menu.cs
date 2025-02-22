using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Entities.Menu
{
    public class Menu : BaseEntity<Guid>
    {
        public Menu()
        {
            IsActive = true;
            IsDelete = false;
            CreateDate = DateTime.Now;
        }
        //public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public int? PageCode { get; set; }
        public string Path { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool? ShowMenu { get; set; }
        public string CssClass { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual ICollection<Menu> InverseParent { get; set; } = new List<Menu>();
        public virtual ICollection<AccessMenus> MenusAccesses { get; set; } = new List<AccessMenus>();
        public virtual ICollection<AccessMenusGroup> MenusGroupAccesses { get; set; } = new List<AccessMenusGroup>();
        public virtual Menu Parent { get; set; }
    }
}
