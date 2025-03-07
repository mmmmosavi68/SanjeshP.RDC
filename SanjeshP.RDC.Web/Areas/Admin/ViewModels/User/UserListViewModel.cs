using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.Areas.Admin.ViewModels.User
{
    public class UserListViewModel
    {
        [Required]
        [Display(Name = "شناسه کاربر")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "نام کاربری الزامی است")]
        [Display(Name = "نام کاربری")]
        [StringLength(50, ErrorMessage = "طول نام کاربری نباید بیش از 50 کاراکتر باشد")]
        public string Username { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "لطفاً یک آدرس ایمیل معتبر وارد کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "نام کامل")]
        [StringLength(100, ErrorMessage = "طول نام کامل نباید بیش از 100 کاراکتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "نقش")]
        public string RoleName { get; set; }

        [Display(Name = "فعال؟")]
        public bool IsActive { get; set; }

        [Display(Name = "آخرین ورود")]
        public string LastLoginDate { get; set; }
    }

}
