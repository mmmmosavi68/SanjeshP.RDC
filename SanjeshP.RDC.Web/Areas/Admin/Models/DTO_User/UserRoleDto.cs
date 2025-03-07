using AutoMapper;
using SanjeshP.RDC.WebFramework.Api;
using System;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Web.Areas.Admin.Models.User;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User
{
    public class UserRoleDto : BaseDto<UserRoleDto, UserRole, int>
    {
        public Guid? UserId { get; set; }
        public int? RoleId { get; set; }
        public virtual RoleDto Role { get; set; }
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
    }
    public class UserRoleSelectDto : BaseDto<UserRoleSelectDto, UserRole, int>
    {
        public string UserName { get; set; }
        public string RoleTitleEn { get; set; }
        public string RoleTitleFa { get; set; }
        public Guid? UserId { get; set; }
        public int? RoleId { get; set; }
        public virtual RoleDto Role { get; set; }
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
        public override void CustomMappings(IMappingExpression<UserRole, UserRoleSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.UserName,
                config => config.MapFrom(src => $"{src.User.UserName}")
                );

            mappingExpression.ForMember(
              dest => dest.RoleTitleEn,
              config => config.MapFrom(src => $"{src.Role.RoleNameFa}")
              );
            mappingExpression.ForMember(
              dest => dest.RoleTitleFa,
              config => config.MapFrom(src => $"{src.Role.RoleNameFa}")
              );
        }
    }
}
