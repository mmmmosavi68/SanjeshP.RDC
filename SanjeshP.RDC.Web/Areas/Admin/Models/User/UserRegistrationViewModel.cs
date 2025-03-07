using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.ViewModels.User
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "نام الزامی است")]
        [MaxLength(50, ErrorMessage = "نام نباید بیش از 50 کاراکتر باشد")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [MaxLength(50, ErrorMessage = "نام خانوادگی نباید بیش از 50 کاراکتر باشد")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "کد ملی الزامی است")]
        [MaxLength(10, ErrorMessage = "کد ملی نباید بیش از 10 کاراکتر باشد")]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "شماره همراه الزامی است")]
        [Phone(ErrorMessage = "شماره همراه معتبر نیست")]
        [Display(Name = "شماره همراه")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "جنسیت الزامی است")]
        [Display(Name = "جنسیت")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "وضعیت فعال بودن الزامی است")]
        [Display(Name = "وضعیت فعال بودن")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "نام کاربری الزامی است")]
        [MaxLength(50, ErrorMessage = "نام کاربری نباید بیش از 50 کاراکتر باشد")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        [MaxLength(100, ErrorMessage = "ایمیل نباید بیش از 100 کاراکتر باشد")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }

    public enum GenderType
    {
        [Display(Name = "مرد")]
        Male = 10,

        [Display(Name = "زن")]
        Female = 11
    }
}
