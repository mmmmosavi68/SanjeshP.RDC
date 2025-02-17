using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Entities.User
{
    public class UserToken : BaseEntity<Guid>   
    {
        //public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string SessionId { get; set; }

        public string UserAgent { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string CreateHostIp { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<UserVerificationCode> UserVerificationCodes { get; set; } = new List<UserVerificationCode>();
    }
}
