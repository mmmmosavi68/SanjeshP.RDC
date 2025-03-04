using System.Collections.Generic;
using System;
using SanjeshP.RDC.WebFramework.Api;
using SanjeshP.RDC.Entities.Group;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_Group
{
    public class GroupDto : BaseDto<GroupDto, Group, Guid>
    {
        public string UserGroupText { get; set; }
        public Guid? CreatorID { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
        public virtual ICollection<UserGroup> GroupUsers { get; set; } = new List<UserGroup>();
    }

    public class GroupSelectDto : BaseDto<GroupSelectDto, Group, Guid>
    {
        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [MaxLength(50, ErrorMessage = "{0} حداکثر 50 کاراکتر میباشد")]
        [Display(Name = "نام گروه")]
        public string UserGroupText { get; set; }
        [Display(Name = "ایجاد کننده")]
        public string CreatorUserName { get; set; }
        public Guid? CreatorId { get; set; }
        [Display(Name = "وضعیت" )]
        public IsActiveTitleType IsActiveTitle { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
        public Guid? CreatorID { get; set; }

        // Map the properties accordingly
        public override void CustomMappings(IMappingExpression<Group, GroupSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.CreatorUserName,
                                         opt => opt.MapFrom(src => src.Creator.UserName));
            mappingExpression.ForMember(dest => dest.CreatorId,
                                         opt => opt.MapFrom(src => src.Creator.Id));
        }
    }
    public enum IsActiveTitleType
    {
        [Display(Name = "غیر فعال")]
        Active = 0,
        [Display(Name = "فعال")]
        Inactive = 1,
    }
}
