using SanjeshP.RDC.Entities.Common;
using System;

namespace SanjeshP.RDC.Entities.User
{
    public class UserVerificationCode : BaseEntity
    {
        //public int Id { get; set; }

        public Guid? TokenId { get; set; }

        public string VerificationCode { get; set; }

        public int VerificationType { get; set; }

        public string VerificationCodeDesc { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual UserToken Token { get; set; }
    }
}
