using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.Areas.Admin.ViewModels.User
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "شناسه کاربر الزامی است")]
        [Display(Name = "شناسه کاربر")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        [Display(Name = "نام")]
        [StringLength(50, ErrorMessage = "طول نام نباید بیش از 50 کاراکتر باشد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [Display(Name = "نام خانوادگی")]
        [StringLength(50, ErrorMessage = "طول نام خانوادگی نباید بیش از 50 کاراکتر باشد")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "کدملی الزامی است")]
        [Display(Name = "کد ملی")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید دقیقاً 10 کاراکتر باشد")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "لطفاً یک آدرس ایمیل معتبر وارد کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "نقش الزامی است")]
        [Display(Name = "نقش")]
        public int RoleId { get; set; }

        [Display(Name = "فعال؟")]
        public bool IsActive { get; set; }
    }

}
