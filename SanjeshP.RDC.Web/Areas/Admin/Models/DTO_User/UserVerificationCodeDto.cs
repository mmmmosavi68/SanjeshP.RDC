using AutoMapper;
using SanjeshP.RDC.WebFramework.Api;
using System;
using SanjeshP.RDC.Entities.User;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User
{
    public class UserVerificationCodeDto : BaseDto<UserVerificationCodeDto, UserVerificationCode, int>
    {
        public Guid? TokenId { get; set; }
        public string VerificationCode { get; set; }
        public int? VerificationType { get; set; }
        public string VerificationCodeDesc { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public virtual UserTokenDto Token { get; set; }
    }

    public class UserVerificationCodeSelectDto : BaseDto<UserVerificationCodeSelectDto, UserVerificationCode, int>
    {
        public string UserName { get; set; }
        public string SessionId { get; set; }
        public string UserAgent { get; set; }
        public Guid? TokenId { get; set; }
        public string VerificationCode { get; set; }
        public int? VerificationType { get; set; }
        public string VerificationCodeDesc { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public virtual UserTokenDto Token { get; set; }
        public override void CustomMappings(IMappingExpression<UserVerificationCode, UserVerificationCodeSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.SessionId,
                config => config.MapFrom(src => $"{src.Token.SessionId}")
                );

            mappingExpression.ForMember(
              dest => dest.UserAgent,
              config => config.MapFrom(src => $"{src.Token.UserAgent}")
              );

            mappingExpression.ForMember(
              dest => dest.UserName,
              config => config.MapFrom(src => $"{src.Token.User.UserName}")
              );
        }
    }
}
