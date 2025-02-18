using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.UserViewModel
{
    public class UserAddViewModel : BaseDto<UserAddViewModel, User, Guid>
    {
        [MaxLength(200)]
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string UserName { get; set; }

        [MaxLength(200)]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string EmailAddress { get; set; }

        [MaxLength(200)]
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(200)]
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string PhoneNumber { get; set; }
    }

    public class UserViewModel : BaseDto<UserViewModel, User, Guid>
    {
        [MaxLength(200)]
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string UserName { get; set; }

        [MaxLength(200)]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string EmailAddress { get; set; }

        [MaxLength(200)]
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(200)]
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "تایید شماره همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public bool? PhoneNumberConfirmed { get; set; }

        [Display(Name = "ورود دو مرحله ای")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public bool? TwoFactorEnabled { get; set; }

        [Display(Name = "وضعیت (فعال/غیرفعال)")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب نمایید.")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        [Display(Name = "آخرین ورود")]
        public DateTimeOffset LastLoginDate { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ انتقضا")]
        public DateTime ExpireDate { get; set; }
    }
}
