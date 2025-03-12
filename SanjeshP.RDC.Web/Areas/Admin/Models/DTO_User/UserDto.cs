using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SanjeshP.RDC.WebFramework.Api;
using SanjeshP.RDC.Entities.User;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User
{
    //public class UserDto : BaseDto<UserDto, SanjeshP.RDC.Entities.User.User, Guid>, IValidatableObject
    //{
    //    //public Guid Id { get; set; }
    //    /// <summary>
    //    /// UserName Min 5 MAx 50 Charachter
    //    /// </summary>
    //    /// <example>Mousavi</example>
    //    [Required(ErrorMessage = "{0} را وارد نمایید.")]
    //    [MaxLength(10, ErrorMessage = "{0} حداکثر 10 کاراکتر میباشد")]
    //    [Display(Name = "نام کاربری")]
    //    public string UserName { get; set; }

    //    [Required(ErrorMessage = "{0}  را وارد نمایید.")]
    //    [MaxLength(50)]
    //    [Display(Name = "Email")]
    //    public string EmailAddress { get; set; }

    //    [Required(ErrorMessage = "{0}  را وارد نمایید.")]
    //    [MaxLength(50)]
    //    [Display(Name = "کلمه عبور")]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }

    //    [Required(ErrorMessage = "{0}  را وارد نمایید.")]
    //    [MaxLength(50)]
    //    [Display(Name = "شماره همراه")]
    //    public string PhoneNumber { get; set; }

    //    //منطق تجاری مثل رمز 123456 نباشد یا مرد بالای 30 سال ثبت نام نکنه
    //    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //    {
    //        if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
    //            yield return new ValidationResult("نام کاربری نمی تواند test باشد", new[] { nameof(UserName) });
    //        if (Password.Equals("123456"))
    //            yield return new ValidationResult("کلمه عبور نمی تواند 123456 باشد", new[] { nameof(Password) });
    //    }
    //    public class UserConfiguration : IEntityTypeConfiguration<SanjeshP.RDC.Entities.User.User>
    //    {
    //        public void Configure(EntityTypeBuilder<SanjeshP.RDC.Entities.User.User> builder)
    //        {
    //            builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
    //        }
    //    }
    //}

    //public class UserSelectDto : BaseDto<UserSelectDto, SanjeshP.RDC.Entities.User.User, Guid>
    //{
    //    //public Guid Id { get; set; }
    //    /// <summary>
    //    /// UserName Min 5 MAx 50 Charachter
    //    /// </summary>
    //    /// <example>Mousavi</example>
    //    [Required(ErrorMessage = "{0} را وارد نمایید.")]
    //    [MaxLength(10, ErrorMessage = "{0} حداکثر 10 کاراکتر میباشد")]
    //    [Display(Name = "نام کاربری")]
    //    public string UserName { get; set; }

    //    [Required(ErrorMessage = "{0}  را وارد نمایید.")]
    //    [MaxLength(50)]
    //    [Display(Name = "Email")]
    //    public string EmailAddress { get; set; }

    //    [Required(ErrorMessage = "{0}  را وارد نمایید.")]
    //    [MaxLength(50)]
    //    [Display(Name = "کلمه عبور")]
    //    public string Password { get; set; }

    //    [Required(ErrorMessage = "{0}  را وارد نمایید.")]
    //    [MaxLength(50)]
    //    [Display(Name = "شماره همراه")]
    //    public string PhoneNumber { get; set; }

    //    public bool IsActive { get; set; }
    //    public bool IsDelete { get; set; }
    //    public DateTimeOffset LastLoginDate { get; set; }
    //    public DateTime CreateDate { get; set; }

    //    public virtual ICollection<UserProfileSelectDto> UserProfiles { get; set; } = new List<UserProfileSelectDto>();

    //    //public virtual ICollection<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();

    //    //public virtual ICollection<UserTablesLogDto> UserTablesLogs { get; set; } = new List<UserTablesLogDto>();

    //    //public virtual ICollection<UserTokenDto> UserTokens { get; set; } = new List<UserTokenDto>();

    //    //public virtual ICollection<AccessMenusGroup> MenusGroupAccesses { get; set; } = new List<AccessMenusGroup>();

    //    //public virtual ICollection<AccessMenuSelectDto> MenusAccesses { get; set; } = new List<AccessMenuSelectDto>();
    //}

    //public class UserSelectByTotalRowCounttDto
    //{
    //    public int TotalRowCounttDto { get; set; }
    //    public ICollection<UserSelectDto> SelectDto { get; set; }
    //}
}
