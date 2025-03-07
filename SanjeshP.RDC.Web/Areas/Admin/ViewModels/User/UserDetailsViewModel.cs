using System.ComponentModel.DataAnnotations;
using System;

namespace SanjeshP.RDC.Web.Areas.Admin.ViewModels.User
{

    public class UserDetailsViewModel
    {
        [Display(Name = "شناسه کاربر")]
        public Guid UserId { get; set; }

        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "نام کامل")]
        public string FullName { get; set; }

        [Display(Name = "نقش")]
        public string RoleName { get; set; }

        [Display(Name = "فعال؟")]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreatedDate { get; set; } // نمایش به صورت رشته

        [Display(Name = "آخرین ورود")]
        public string LastLoginDate { get; set; } // نمایش به صورت رشته
    }
}
