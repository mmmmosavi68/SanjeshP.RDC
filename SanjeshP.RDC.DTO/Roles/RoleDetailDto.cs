using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanjeshP.RDC.DTO.Roles
{
    public class RoleDetailDto
    {
        public int RoleId { get; set; } // شناسه نقش

        [Display(Name = "نام نقش")]
        public string RoleName { get; set; }

        [Display(Name = "شرح نقش")]
        public string Description { get; set; }

        [Display(Name = "کاربران")]
        public List<UserRoleDto> Users { get; set; } // لیست کاربران متصل به این نقش
    }

}
