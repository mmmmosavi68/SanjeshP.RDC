using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Web.Areas.Admin.Models;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.Collections.Generic;

namespace MMM.Olympiad.Web.Areas.UserArea.Models
{
    public class ListMenuDto : BaseDto<ListMenuDto, Menu, Guid>
    {
        public ListMenuDto()
        {
            InverseParent = new HashSet<ListMenu>();
            UserAccessMenus = new HashSet<AccessMenus>();
            UserAccessMenusGroups = new HashSet<AccessMenusGroup>();
        }
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public int? PageCode { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool? ShowMenu { get; set; }
        public string CssClass { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public List<ListMenuDto> children { get; set; }
        public virtual ListMenu Parent { get; set; }
        public virtual ICollection<ListMenu> InverseParent { get; set; }
        public virtual ICollection<AccessMenus> UserAccessMenus { get; set; }
        public virtual ICollection<AccessMenusGroup> UserAccessMenusGroups { get; set; }
    }

    public class ListMenuUserAccessDto
    {
        public Guid id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public bool Person_Checkecd { get; set; }

        public ListMenuUserAccessStateDto state { get; set; }
    }
    public class ListMenuUserAccessStateDto
    {
        public bool opened { get; set; }
        public bool selected { get; set; }
    }

    //public class UserAccessMenuDto : BaseDto<UserAccessMenuDto, UserAccessMenus, int>
    //{
    //    public Guid? UserId { get; set; }
    //    public Guid? ListMenuId { get; set; }
    //    public Guid? Creator { get; set; }
    //    public bool? IsDelete { get; set; }
    //    public DateTime? CreateDate { get; set; }
    //    public string HostIp { get; set; }

    //    public virtual List<ListMenuDto> ListMenu { get; set; }
    //    public virtual Users User { get; set; }
    //}
}
