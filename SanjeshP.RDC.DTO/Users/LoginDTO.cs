using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

}
