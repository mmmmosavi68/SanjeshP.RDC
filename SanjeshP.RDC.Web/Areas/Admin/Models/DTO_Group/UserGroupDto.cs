using AutoMapper;
using SanjeshP.RDC.Entities.Group;
using SanjeshP.RDC.Entities.Menu;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Group
{
    public class UserGroupDto : BaseDto<UserGroupDto, UserGroup, Guid>
    {
        public Guid? UserId { get; set; }
        public Guid? GroupId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<AccessMenusGroup> MenusGroupAccesses { get; set; } = new List<AccessMenusGroup>();
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
        public virtual Group Group { get; set; }
    }

    public class UserGroupSelectDto : BaseDto<UserGroupSelectDto, UserGroup, Guid>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string NationalCode { get; set; }
        public string UserGroupText { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<AccessMenusGroup> MenusGroupAccesses { get; set; } = new List<AccessMenusGroup>();
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
        public virtual Group Group { get; set; }

        public override void CustomMappings(IMappingExpression<UserGroup, UserGroupSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
               dest => dest.FullName,
               config => config.MapFrom(src => $"{User.UserProfiles.Select(U => U.FirstName).FirstOrDefault()}" + " " + $"{User.UserProfiles.Select(U => U.LastName).FirstOrDefault()}")
               );

            mappingExpression.ForMember(
                dest => dest.UserName,
                config => config.MapFrom(src => $"{User.UserName}")
                );

            mappingExpression.ForMember(
               dest => dest.NationalCode,
               config => config.MapFrom(src => $"{User.UserProfiles.Select(U => U.NationalCode).FirstOrDefault()}")
               );

            mappingExpression.ForMember(
              dest => dest.UserGroupText,
              config => config.MapFrom(src => $"{src.Group.UserGroupText}")
              );
        }
    }
}
