﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class UserListDto
    {
        public Guid UserId { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        public string EmailAddress { get; set; }

        [Display(Name = "عنوان نقش")]
        public string RoleTitle { get; set; } // استخراج از جدول Role

        [Display(Name = "فعال؟")]
        public bool IsActive { get; set; }

        [Display(Name = "وضعیت")]
        public string IsActiveTitle { get; set; } // استخراج وضعیت به صورت متنی

        [Display(Name = "حذف شده؟")]
        public bool IsDelete { get; set; }
    }
}