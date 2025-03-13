﻿using SanjeshP.RDC.Common.MyAttribute;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.Areas.Admin.ViewModels.Groups
{
    public class GroupEditViewModel : BaseDto<GroupEditViewModel, Entities.Group.Group, Guid>
    {
        public Guid GroupId { get; set; }

        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [PersianEnglishInputValidator(ErrorMessage = "لطفاً فقط از حروف فارسی، انگلیسی، اعداد و کاراکترهای مجاز استفاده کنید.")]
        [Display(Name = "نام گروه")]
        public string GroupName { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }
    }
}
