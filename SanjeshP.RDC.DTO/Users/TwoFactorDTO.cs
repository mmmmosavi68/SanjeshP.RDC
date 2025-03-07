using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class TwoFactorDTO
    {
        public string UserId { get; set; }
        public string VerificationCode { get; set; }
    }

}
