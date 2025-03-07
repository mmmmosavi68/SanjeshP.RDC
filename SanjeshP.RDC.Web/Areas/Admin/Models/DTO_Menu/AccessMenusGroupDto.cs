using AutoMapper;
using SanjeshP.RDC.Entities.Group;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.WebFramework.Api;
using System;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu
{
    public class AccessMenusGroupDto : BaseDto<AccessMenusGroupDto, GroupAccessMenus, int>
    {
        public Guid? GroupId { get; set; }
        public Guid? ListMenuId { get; set; }
        public Guid? Creator { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
        public virtual Menu ListMenu { get; set; }
        public virtual GroupUsers UserGroup { get; set; }
    }

    public class AccessMenusGroupSelectDto : BaseDto<AccessMenusGroupSelectDto, GroupAccessMenus, int>
    {
        public string GroupTitle { get; set; }
        public string MenuTitle { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? ListMenuId { get; set; }
        public Guid? Creator { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
        public virtual Menu ListMenu { get; set; }
        public virtual GroupUsers UserGroup { get; set; }

        public override void CustomMappings(IMappingExpression<GroupAccessMenus, AccessMenusGroupSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.GroupTitle,
                config => config.MapFrom(src => $"{UserGroup.Group.GroupName}")
                );

            mappingExpression.ForMember(
                dest => dest.MenuTitle,
                config => config.MapFrom(src => $"{ListMenu.MenuTitle}")
                );
        }
    }
}
