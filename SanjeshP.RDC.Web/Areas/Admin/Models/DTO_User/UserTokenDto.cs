using AutoMapper;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.Collections.Generic;
using SanjeshP.RDC.Entities.User;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User
{
    public class UserTokenDto : BaseDto<UserTokenDto, UserToken, Guid>
    {
        //public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string SessionId { get; set; }

        public string UserAgent { get; set; }

        public bool? IsDelete { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string CreateHostIp { get; set; }

        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }

        public virtual ICollection<UserVerificationCodeDto> UserVerificationCodes { get; set; } = new List<UserVerificationCodeDto>();
    }

    public class UserTokenSelectDto : BaseDto<UserTokenSelectDto, UserToken, Guid>
    {
        public string  UserName { get; set; }
        public Guid UserId { get; set; }
        public string SessionId { get; set; }
        public string UserAgent { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CreateHostIp { get; set; }
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
        public virtual ICollection<UserVerificationCodeDto> UserVerificationCodes { get; set; } = new List<UserVerificationCodeDto>();
        public override void CustomMappings(IMappingExpression<UserToken, UserTokenSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.UserName,
                config => config.MapFrom(src => $"{src.User.UserName}")
                );
        }
    }
}
