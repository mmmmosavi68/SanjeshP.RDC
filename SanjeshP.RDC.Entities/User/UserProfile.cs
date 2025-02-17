using SanjeshP.RDC.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.Entities.User
{
    public class UserProfile : BaseEntity
    {
        public UserProfile()
        {
            IsActive = true;
            IsDelete = false;
            CreateDate = DateTime.Now;
        }
        //public int Id { get; set; }
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
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual User User { get; set; }
    }

    public enum GenderType
    {
        [Display(Name = "مرد")]
        Male = 10,

        [Display(Name = "زن")]
        Female = 11
    }

    public enum MaritalStatusType
    {
        [Display(Name = "متاهل")]
        Single = 10,

        [Display(Name = "مجرد")]
        Married = 11
    }

    public enum LeftHandedType
    {
        [Display(Name = "راست دست")]
        Right = 0,

        [Display(Name = "چپ دست")]
        Left = 11
    }
}
