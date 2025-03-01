using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.WebFramework.Api;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu
{
    public class MenuDto : BaseDto<MenuDto, Menu, Guid>
    {
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public int? PageCode { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool? ShowMenu { get; set; }
        public bool Person_Checkecd { get; set; }
        public bool Group_Checkecd { get; set; }
        public string CssClass { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public virtual ICollection<MenuDto> InverseParent { get; set; } = new List<MenuDto>();
        public virtual MenuDto Parent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<MenuDto> children { get; set; }
    }

    public class MenuSelectDto : BaseDto<MenuSelectDto, Menu, Guid>
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
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public ICollection<MenuSelectDto> menuSelectDtos { get; set; }
    }

    public class MenuSelectTreeJsDto
    {
        public string key { get; set; }
        public string title { get; set; }
        public ICollection<MenuSelectTreeJsDto> children { get; set; }
        public bool disabled { get; set; }
        public bool disableCheckbox { get; set; }

    }


    public class UserAccessMenuWithMenubar
    {
        public ICollection<MenuSelectTreeJsDto> menuSelectTreeJsDtos { get; set; }
        public List<string> UserAccessMenu { get; set; }
    }

    #region MyRegion
    public class ListMenuUserAccessDto
    {
        public Guid id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public JStreeAttr a_attr { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public bool Person_Checkecd { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public bool Group_Checkecd { get; set; }

        public ListMenuUserAccessStateDto state { get; set; }
    }
    public class ListMenuUserAccessStateDto
    {
        public bool opened { get; set; }
        public bool selected { get; set; }
        public bool disabled { get; set; }

    }

    public class JStreeAttr
    {
        public string href { get; set; }
        public string title { get; set; }
    }
    #endregion Fore Tree view js
}
