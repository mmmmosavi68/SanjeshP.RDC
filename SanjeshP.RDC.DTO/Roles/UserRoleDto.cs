using System;
using System.ComponentModel.DataAnnotations;

namespace SanjeshP.RDC.DTO.Roles
{
    public class UserRoleDto
    {
        public Guid UserId { get; set; } // شناسه کاربر

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; } // نام کاربر از جدول User

        public int RoleId { get; set; } // شناسه نقش

        [Display(Name = "نام نقش")]
        public string RoleName { get; set; } // نام نقش از جدول Role
    }

}
