using AutoMapper;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.Linq;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Menu
{
    public class AccessMenuDto : BaseDto<AccessMenuDto, AccessMenus>
    {
        public Guid? UserId { get; set; }
        public Guid? ListMenuId { get; set; }
        public Guid? Creator { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
        public virtual Menu ListMenu { get; set; }
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
    }

    public class AccessMenuSelectDto : BaseDto<AccessMenuSelectDto, AccessMenus>
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MenuTitle { get; set; }
        public Guid? ListMenuId { get; set; }
        public Guid? Creator { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
        public virtual Menu ListMenu { get; set; }
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
        public override void CustomMappings(IMappingExpression<AccessMenus, AccessMenuSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.UserName,
                config => config.MapFrom(src => $"{User.UserName}")
                );

            mappingExpression.ForMember(
               dest => dest.FullName,
               config => config.MapFrom(src => $"{User.UserProfiles.Select(U => U.FirstName).FirstOrDefault()}" + " " + $"{User.UserProfiles.Select(U => U.LastName).FirstOrDefault()}")
               );
            mappingExpression.ForMember(
              dest => dest.MenuTitle,
              config => config.MapFrom(src => $"{src.ListMenu.Title}")
              );
        }
    }
}
