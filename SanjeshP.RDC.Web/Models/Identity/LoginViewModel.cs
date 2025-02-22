using AutoMapper;
using SanjeshP.RDC.Common.MyAttribute;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.Models.Identity
{
    public class LoginDTO
    {
        [MaxLength(200)]
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string UserName { get; set; }

        [MaxLength(200)]
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "مرا به خاطر بسپار")]
        public bool RemmeberMe { get; set; }

        //[Display(Name = "تکرار کلمه عبور")]
        //[Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        //[Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        //[DataType(DataType.Password)]
        //public string RePassword { get; set; }
    }

    public class RegisterDTO : BaseDto<RegisterDTO, User, Guid>
    {
        [MaxLength(100, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string LastName { get; set; }

        [MaxLength(10, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Text)]
        [NationalCode("لطفا کد ملی را بدرستی وارد کنید")]
        public string NationalCode { get; set; }

        [MaxLength(11, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public override void CustomMappings(IMappingExpression<User, RegisterDTO> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.NationalCode, opt => opt.MapFrom(src => src.UserName));
        }
    }

    public class RecoveryPasswordDTO : BaseDto<RecoveryPasswordDTO, User, Guid>
    {
        [MaxLength(200)]
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Text)]
        public string NationalCode { get; set; }

        [MaxLength(200)]
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
    }
}
