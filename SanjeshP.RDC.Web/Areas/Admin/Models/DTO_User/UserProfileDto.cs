using AutoMapper;
using SanjeshP.RDC.Common.MyAttribute;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User
{
    //public class RegisterEditDto 
    //{
    //    [Required]
    //    public Guid UserId { get; set; }

    //    [MaxLength(100, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
    //    [Display(Name = "نام")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    public string FirstName { get; set; }

    //    [MaxLength(100, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
    //    [Display(Name = "نام خانوادگی")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    public string LastName { get; set; }

    //    [MaxLength(10, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
    //    [Display(Name = "کد ملی")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    [DataType(DataType.Text)]
    //    [NationalCode("لطفا کد ملی را بدرستی وارد کنید")]
    //    public string NationalCode { get; set; }

    //    [MaxLength(200)]
    //    [Display(Name = "نام کاربری")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
    //    public string UserName { get; set; }

    //    [MaxLength(200)]
    //    [Display(Name = "کلمه عبور")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
    //    public string Password { get; set; }

    //    [MaxLength(200)]
    //    [Display(Name = "ایمیل")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
    //    public string EmailAddress { get; set; }

    //    [MaxLength(11, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
    //    [Display(Name = "شماره همراه")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    [DataType(DataType.PhoneNumber)]
    //    public string PhoneNumber { get; set; }

    //    [Display(Name = "نوع کاربری")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
    //    public int? RoleId { get; set; }

    //    [Display(Name = "وضعیت ( فعال/غیرفعال)")]
    //    public bool IsActive { get; set; }
    //}
    //public class RegisterDto : BaseDto<RegisterDto, UserProfile>
    //{
    //    [Required]
    //    public Guid UserId { get; set; }

    //    [MaxLength(100, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
    //    [Display(Name = "نام")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    public string FirstName { get; set; }

    //    [MaxLength(100, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
    //    [Display(Name = "نام خانوادگی")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    public string LastName { get; set; }

    //    [MaxLength(10, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
    //    [Display(Name = "کد ملی")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    [DataType(DataType.Text)]
    //    [NationalCode("لطفا کد ملی را بدرستی وارد کنید")]
    //    public string NationalCode { get; set; }

    //    [MaxLength(200)]
    //    [Display(Name = "نام کاربری")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
    //    public string UserName { get; set; }

    //    [MaxLength(200)]
    //    [Display(Name = "کلمه عبور")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
    //    public string Password { get; set; }

    //    [MaxLength(200)]
    //    [Display(Name = "ایمیل")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
    //    public string EmailAddress { get; set; }

    //    [MaxLength(11, ErrorMessage = "{0} حداکثر {1} کاراکتر میباشد.")]
    //    [Display(Name = "شماره همراه")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    [DataType(DataType.PhoneNumber)]
    //    public string PhoneNumber { get; set; }

    //    [Display(Name = "نوع کاربری")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
    //    public int? RoleId { get; set; }

    //    [Display(Name = "نوع کاربری")]
    //    public string UserTypeTitle { get; set; }

    //    [Display(Name = "وضعیت ( فعال/غیرفعال)")]
    //    public bool IsActive { get; set; }

    //    [Display(Name = "وضعیت ( فعال/غیرفعال)")]
    //    public IsActiveTitleType IsActiveTitle { get; set; }
    //    public bool IsDelete { get; set; }
    //    public Guid Creator { get; set; }
    //    public string HostIp { get; set; }
    //}

    //public enum IsActiveTitleType
    //{
    //    [Display(Name = "غیر فعال")]
    //    Active = 0,
    //    [Display(Name = "فعال")]
    //    Inactive = 1,
    //}

    //public class UserProfileDto : BaseDto<UserProfileDto, UserProfile, int>
    //{
    //    public Guid UserId { get; set; }

    //    public string FirstName { get; set; }

    //    public string LastName { get; set; }

    //    public string FatherName { get; set; }

    //    public string NationalCode { get; set; }

    //    public GenderType? Gender { get; set; }

    //    public MaritalStatusType? MaritalStatus { get; set; }

    //    public int? Religion { get; set; }

    //    public LeftHandedType? LeftHanded { get; set; }

    //    public int? Nationality { get; set; }

    //    //public virtual User User { get; set; }
    //}

    //public class UserProfileSelectDto : BaseDto<UserProfileSelectDto, UserProfile, int>
    //{
    //    public Guid UserId { get; set; }

    //    public string FirstName { get; set; }

    //    public string LastName { get; set; }

    //    public string FullName { get; set; }

    //    public string FatherName { get; set; }

    //    public string NationalCode { get; set; }

    //    public GenderType? Gender { get; set; }

    //    public MaritalStatusType? MaritalStatus { get; set; }

    //    public int? Religion { get; set; }

    //    public LeftHandedType? LeftHanded { get; set; }

    //    public int? Nationality { get; set; }

    //    //public virtual User User { get; set; }

    //    public override void CustomMappings(IMappingExpression<UserProfile, UserProfileSelectDto> mappingExpression)
    //    {
    //        mappingExpression.ForMember(
    //            dest => dest.FullName,
    //            config => config.MapFrom(src => $"{src.FirstName}" + " " + $"{src.LastName}" + " " + $"{src.User.PhoneNumber}")
    //            );
    //    }
    //}
}

