using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class UserDetailsDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

}
