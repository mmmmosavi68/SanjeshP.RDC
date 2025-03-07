using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.Models.Identity
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        [MaxLength(50, ErrorMessage = "نام کاربری نباید بیش از 50 کاراکتر باشد")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
    }
}