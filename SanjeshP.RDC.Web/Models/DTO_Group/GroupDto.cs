using System.Collections.Generic;
using System;
using SanjeshP.RDC.WebFramework.Api;
using SanjeshP.RDC.Entities.Group;

namespace SanjeshP.RDC.Models.DTO_Group
{
    public class GroupDto : BaseDto<GroupDto, Group, Guid>
    {
        public string UserGroupText { get; set; }
        public Guid? Creator { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
        public virtual ICollection<UserGroup> GroupUsers { get; set; } = new List<UserGroup>();
    }

    public class GroupSelectDto : BaseDto<GroupSelectDto, Group, Guid>
    {
        public string UserGroupText { get; set; }
        public string CreatorUserName { get; set; }
        public Guid? Creator { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
    }

}
