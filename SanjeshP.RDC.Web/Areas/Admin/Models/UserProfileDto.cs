using System.ComponentModel.DataAnnotations;
using System;
using SanjeshP.RDC.WebFramework.Api;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.Common.MyAttribute;

namespace SanjeshP.RDC.Web.Areas.Admin.Models
{
    public class UserProfileDto
    {

    }

    public class RegisterDto : BaseDto<RegisterDto, UserProfile>
    {
        [Required]
        public Guid UserId { get; set; }

        [MaxLength(100, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string LastName { get; set; }

        [MaxLength(10, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Text)]
        [NationalCode("لطفا کد ملی را بدرستی وارد کنید")]
        public string NationalCode { get; set; }

        [MaxLength(200)]
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string UserName { get; set; }

        [MaxLength(200)]
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Password { get; set; }

        [MaxLength(200)]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string EmailAddress { get; set; }

        [MaxLength(11, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "نوع کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int? RoleId { get; set; }

        [Display(Name = "نوع کاربری")]
        public string UserTypeTitle { get; set; }

        [Display(Name = "وضعیت ( فعال/غیرفعال)")]
        public bool IsActive { get; set; }

        [Display(Name = "وضعیت ( فعال/غیرفعال)")]
        public IsActiveTitleType IsActiveTitle { get; set; }
        public bool IsDelete { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
    }

    public enum IsActiveTitleType
    {
        [Display(Name = "غیر فعال")]
        Active = 0,
        [Display(Name = "فعال")]
        Inactive = 1,
    }

}
