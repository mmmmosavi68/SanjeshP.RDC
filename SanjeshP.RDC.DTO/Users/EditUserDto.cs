using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class EditUserDto
    {
        [Required(ErrorMessage = "شناسه کاربر الزامی است")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        [StringLength(50, ErrorMessage = "طول نام نباید بیش از 50 کاراکتر باشد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [StringLength(50, ErrorMessage = "طول نام خانوادگی نباید بیش از 50 کاراکتر باشد")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "کدملی الزامی است")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید دقیقاً 10 کاراکتر باشد")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "لطفاً یک آدرس ایمیل معتبر وارد کنید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "شناسه نقش الزامی است")]
        public int RoleId { get; set; }

        public bool IsActive { get; set; }
    }
}
