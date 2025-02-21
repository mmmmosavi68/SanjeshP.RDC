using SanjeshP.RDC.Common;
using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Entities.Menu
{
    public class View_UserMenubar : BaseEntity<Guid>, IIgnoreDependency
    {
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public int? PageCode { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool? ShowMenu { get; set; }
        public string CssClass { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public bool CurentUser_Person_Checkecd { get; set; }
        public bool CurentUser_Group_Checkecd { get; set; }
        public int IsParent { get; set; }
        public bool Person_Checkecd { get; set; }
        public bool Group_Checkecd { get; set; }
        public bool disabled { get; set; }
    }
}
