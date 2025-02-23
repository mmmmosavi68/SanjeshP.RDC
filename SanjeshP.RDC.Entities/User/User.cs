using SanjeshP.RDC.Entities.Common;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Entities.User
{
    public class User : BaseEntity<Guid>
    {
        public User()
        {
            IsActive = true;
            IsDelete = false;
            SecurityStamp = Guid.NewGuid();
            CreateDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddYears(1);
        }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string EmailAddress { get; set; }
        public string NormalizedEmailAddress { get; set; }
        public bool? EmailAddressConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public Guid? SecurityStamp { get; set; }
        public Guid? ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }



        public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public virtual ICollection<UserTablesLog> UserTablesLogs { get; set; } = new List<UserTablesLog>();

        public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();

        public virtual ICollection<AccessMenusGroup> MenusGroupAccesses { get; set; } = new List<AccessMenusGroup>();

        public virtual ICollection<AccessMenus> MenusAccesses { get; set; } = new List<AccessMenus>();

    }
}
