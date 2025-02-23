using AutoMapper;

using SanjeshP.RDC.WebFramework.Api;
using System;
using System.Collections.Generic;
using SanjeshP.RDC.Entities.User;

namespace SanjeshP.RDC.Web.Areas.Admin.Models.DTO_User
{
    public class UserTablesLogDto : BaseDto<UserTablesLogDto, UserTablesLog, int>
    {
        public Guid? UserId { get; set; }
        public string TableName { get; set; }
        public string RecordId { get; set; }
        public int? Opareation { get; set; }
        public string CreateHostIp { get; set; }
        public DateTime? CreateDate { get; set; }
        public string SystemInformation { get; set; }
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
        public virtual ICollection<UserTableLogInformationDto> UserTableLogInformations { get; set; } = new List<UserTableLogInformationDto>();
    }
    public class UserTablesLogSelectDto : BaseDto<UserTablesLogSelectDto, UserTablesLog, int>
    {
        public string UserName { get; set; }
        public Guid? UserId { get; set; }
        public string TableName { get; set; }
        public string RecordId { get; set; }
        public int? Opareation { get; set; }
        public string CreateHostIp { get; set; }
        public DateTime? CreateDate { get; set; }
        public string SystemInformation { get; set; }
        public virtual SanjeshP.RDC.Entities.User.User User { get; set; }
        public virtual ICollection<UserTableLogInformationDto> UserTableLogInformations { get; set; } = new List<UserTableLogInformationDto>();

        public override void CustomMappings(IMappingExpression<UserTablesLog, UserTablesLogSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.UserName,
                config => config.MapFrom(src => $"{src.User.UserName}")
                );
        }
    }
}
