using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class ResetPasswordDTO
    {
        public string UserId { get; set; }
        public string VerificationCode { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }

}
