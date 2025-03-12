using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.DTO.Roles
{
    public class RoleDto
    {
        public int RoleId { get; set; } // شناسه نقش

        [Required(ErrorMessage = "{0} را وارد نمایید.")]
        [MaxLength(50, ErrorMessage = "{0} حداکثر 50 کاراکتر میباشد.")]
        [Display(Name = "نام نقش")]
        public string RoleName { get; set; } // نام نقش

        [Display(Name = "شرح نقش")]
        public string Description { get; set; } // توضیحات نقش

        [Display(Name = "تعداد کاربران متصل")]
        public int UserCount { get; set; } // تعداد کاربرانی که این نقش را دارند
    }

}
