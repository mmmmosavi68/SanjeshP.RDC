using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [Display(Name = "ایمیل")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [Display(Name = "نقش")]
        public int? RoleId { get; set; } // مشخص کردن نقش کاربر

        public Guid Creator { get; set; }

        public string HostIp { get; set; }
    }
}
