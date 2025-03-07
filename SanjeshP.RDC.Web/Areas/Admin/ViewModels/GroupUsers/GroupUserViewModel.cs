using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Web.Areas.Admin.ViewModels.GroupUsers
{
    public class GroupUserViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string GroupName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
