using AutoMapper;
using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.WebFramework.Api;
using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Models.DTO_User
{
    public class UserProfileDto : BaseDto<UserProfileDto, UserProfile, int>
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public string NationalCode { get; set; }

        public GenderType? Gender { get; set; }

        public MaritalStatusType? MaritalStatus { get; set; }

        public int? Religion { get; set; }

        public LeftHandedType? LeftHanded { get; set; }

        public int? Nationality { get; set; }

        //public virtual User User { get; set; }
    }

    public class UserProfileSelectDto : BaseDto<UserProfileSelectDto, UserProfile, int>
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string FatherName { get; set; }

        public string NationalCode { get; set; }

        public GenderType? Gender { get; set; }

        public MaritalStatusType? MaritalStatus { get; set; }

        public int? Religion { get; set; }

        public LeftHandedType? LeftHanded { get; set; }

        public int? Nationality { get; set; }

        //public virtual User User { get; set; }

        public override void CustomMappings(IMappingExpression<UserProfile, UserProfileSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.FullName,
                config => config.MapFrom(src => $"{src.FirstName}" + " " + $"{src.LastName}" + " " + $"{src.User.PhoneNumber}")
                );
        }
    }
}

