using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class AddUserDto
    {
        [Required(ErrorMessage = "نام الزامی است")]
        [StringLength(50, ErrorMessage = "طول نام نباید بیش از 50 کاراکتر باشد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [StringLength(50, ErrorMessage = "طول نام خانوادگی نباید بیش از 50 کاراکتر باشد")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "کدملی الزامی است")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کدملی باید دقیقاً 10 کاراکتر باشد")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "نام کاربری الزامی است")]
        [StringLength(50, ErrorMessage = "طول نام کاربری نباید بیش از 50 کاراکتر باشد")]
        public string Username { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "لطفاً یک آدرس ایمیل معتبر وارد کنید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید بین 6 تا 100 کاراکتر باشد")]
        public string Password { get; set; }

        [Required(ErrorMessage = "شناسه نقش الزامی است")]
        public int RoleId { get; set; }

        public bool IsActive { get; set; }
    }

}
