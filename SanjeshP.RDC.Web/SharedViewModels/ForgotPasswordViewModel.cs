using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.SharedViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "لطفاً ایمیل یا شماره تلفن را وارد کنید.")]
        public string EmailOrPhoneNumber { get; set; }
    }

}
