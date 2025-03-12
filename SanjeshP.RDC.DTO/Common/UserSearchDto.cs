using System;
using System.Collections.Generic;
using System.Text;

namespace SanjeshP.RDC.DTO.Common
{
    public class UserSearchDto
    {
        public string SearchTerm { get; set; } // عبارت جستجو (مثل نام یا ایمیل)
        public int PageNumber { get; set; } = 1; // شماره صفحه (پیش‌فرض 1)
        public int PageSize { get; set; } = 10; // اندازه صفحه (پیش‌فرض 10)
    }

}
