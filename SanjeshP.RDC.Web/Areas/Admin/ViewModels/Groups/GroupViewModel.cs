using System.ComponentModel.DataAnnotations;
using System;
using SanjeshP.RDC.Common.MyAttribute;
using SanjeshP.RDC.WebFramework.Api;
using SanjeshP.RDC.Web.Areas.Admin.ViewModels.User;

namespace SanjeshP.RDC.Web.Areas.Admin.ViewModels.Groups
{
    public class GroupViewModel :BaseDto<GroupViewModel,Entities.Group.Group,Guid>
    {
        [Display(Name = "نام گروه")]
        public string GroupName { get; set; }
        [Display(Name = "ایجاد کننده")]
        public string CreatorUserName { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "وضعیت ( فعال/غیرفعال)")]
        public IsActiveNameType IsActiveName { get; set; }
    }
}
