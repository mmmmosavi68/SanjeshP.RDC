using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.DTO.Users
{
    public class UserListDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

}

//UserId: شناسه کاربر از جدول User.

//Username: نام کاربری.

//Email: ایمیل کاربر از جدول User.

//FullName: نام کامل از جدول UserProfile.

//RoleName: نام نقش کاربر از جدول Role.

//IsActive: وضعیت فعال/غیرفعال کاربر.

//LastLoginDate: آخرین تاریخ ورود.
