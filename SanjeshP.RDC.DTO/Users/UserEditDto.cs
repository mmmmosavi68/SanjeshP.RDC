using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class UserEditDto
    {
        public Guid UserId { get; set; }

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

        [Display(Name = "ایمیل")]
        public string EmailAddress { get; set; }

        [Display(Name = "نقش")]
        public int? RoleId { get; set; }

        [Display(Name = "فعال؟")]
        public bool IsActive { get; set; }

        public string HostIp { get; set; }
    }
}
